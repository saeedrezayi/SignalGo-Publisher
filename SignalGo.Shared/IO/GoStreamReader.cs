﻿using SignalGo.Shared.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if (!PORTABLE)
using System.Net.Sockets;
#endif
using System.Text;
using System.Threading;

namespace SignalGo.Shared.IO
{
    /// <summary>
    /// read signalGo blocks of stream
    /// </summary>
    public static class GoStreamReader
    {
        /// <summary>
        /// read data from stream
        /// </summary>
        /// <param name="stream">your stream to read a block</param>
        /// <param name="compress">compress mode</param>
        /// <returns>return a block byte array</returns>
        public static byte[] ReadBlockToEnd(Stream stream, CompressMode compress, uint maximum, bool isWebSocket)
        {
            if (isWebSocket)
            {
                ulong size = 0;
                var bytes = GetLengthOfWebSocket(stream, ref size);
                //#if (!PORTABLE)
                //                Console.WriteLine("read webSocket converted bytes: " + bytes.Count);
                //                foreach (var item in bytes)
                //                {
                //                    Console.WriteLine(item);
                //                }
                //                Console.WriteLine("end webSocket converted bytes");
                //#endif
                var newBytes = ReadBlockSize(stream, size);
                List<byte> b = new List<byte>();
                b.AddRange(bytes);
                b.AddRange(newBytes);
                var decode = DecodeMessage(b.ToArray());
                if (decode == null || decode.Length == 0)
                {
                    throw new Exception("websocket closed by client");
                    //AutoLogger.LogText($"decode zero size: {size} bytes: {ByteArrayToText(bytes.ToArray())} newBytes:{ByteArrayToText(newBytes)} decode: {ByteArrayToText(decode)}");
                }
                return decode;
            }
            else
            {
                //first 4 bytes are size of block
                var dataLenByte = ReadBlockSize(stream, 4);
                //convert bytes to int
                int dataLength = BitConverter.ToInt32(dataLenByte, 0);
                if (dataLength > maximum)
                    throw new Exception("dataLength is upper than maximum :" + dataLength + " " + isWebSocket);
                //else
                //    AutoLogger.LogText("size: " + dataLength);
                //read a block
                var dataBytes = ReadBlockSize(stream, (ulong)dataLength);
                return dataBytes;
            }
        }

        static string ByteArrayToText(byte[] bytes)
        {
            if (bytes == null)
                return "null";
            else if (bytes.Length == 0)
                return "zero!";
            string result = "";
            foreach (var item in bytes)
            {
                if (result != "")
                    result += "," + item;
                else
                    result += item;
            }
            return result;
        }
        /// <summary>
        /// read one byte from server
        /// </summary>
        /// <param name="stream">stream to read</param>
        /// <param name="compress">compress mode</param>
        /// <param name="maximum">maximum read</param>
        /// <param name="isWebSocket">if reading socket is websocket</param>
        /// <returns></returns>
        public static byte ReadOneByte(Stream stream, CompressMode compress, uint maximum, bool isWebSocket)
        {
            byte[] dataBytes = null;
            if (isWebSocket)
                dataBytes = ReadBlockToEnd(stream, compress, maximum, isWebSocket);
            else
            {
                var data = stream.ReadByte();
                if (data < 0)
                    throw new Exception($"read one byte is correct or disconnected client! {data}");
                return (byte)data;
            }
            return dataBytes[0];
        }

        public static byte ReadOneByte(Stream stream, TimeSpan timeout)
        {
            ManualResetEvent resetEvent = new ManualResetEvent(false);
            resetEvent.Reset();
            byte oneByte = 0;
            AsyncActions.Run(() =>
            {
                var data = stream.ReadByte();
                if (data >= 0)
                {
                    oneByte = (byte)data;
                    resetEvent.Set();
                }
            });

            if (resetEvent.WaitOne(timeout))
                return oneByte;
            throw new TimeoutException();
        }

        static List<byte> GetLengthOfWebSocket(Stream stream, ref ulong len)
        {
            List<byte> bytes = new List<byte>();

            bytes.AddRange(ReadBlockSize(stream, 2));
            //#if (!PORTABLE)
            //            Console.WriteLine("read block bytes: " + bytes.Count);
            //            foreach (var item in bytes)
            //            {
            //                Console.WriteLine(item);
            //            }
            //            Console.WriteLine("end read block bytes");
            //#endif

            if (bytes[1] - 128 <= 125)
            {
                len = (ulong)bytes[1] - 128;
                len += 4;
            }
            else if (bytes[1] - 128 == 126)
            {
                var newSize = ReadBlockSize(stream, 2);
                bytes.AddRange(newSize);
                len = BitConverter.ToUInt16(new byte[2] { newSize[1], newSize[0] }, 0);
                len += 4;
            }
            else if (bytes[1] - 128 == 127)
            {
                var newSize = ReadBlockSize(stream, 8);
                bytes.AddRange(newSize);
                len = BitConverter.ToUInt16(new byte[8] { bytes[7], newSize[6], newSize[5], newSize[4], newSize[3], newSize[2], newSize[1], newSize[0] }, 0);
                len += 4;
            }
            else
            {
                throw new Exception("incorrect websocket data");
            }
            return bytes;
        }

        public static byte[] ReadBlockSize(Stream stream, ulong count)
        {
            List<byte> bytes = new List<byte>();
            ulong lengthReaded = 0;

            while (lengthReaded < count)
            {
                ulong countToRead = count;
                if (lengthReaded + countToRead > count)
                {
                    countToRead = count - lengthReaded;
                }
                //#if (!PORTABLE)
                //                Console.WriteLine("countToRead: " + countToRead);
                //#endif
                byte[] readBytes = new byte[countToRead];
                var readCount = stream.Read(readBytes, 0, (int)countToRead);
                if (readCount <= 0)
                    throw new Exception("read zero buffer! client disconnected: " + readCount);
                lengthReaded += (ulong)readCount;
                bytes.AddRange(readBytes.ToList().GetRange(0, readCount));
            }
            return bytes.ToArray();
        }

#if (!PORTABLE)
        public static byte[] ReadBlockSizeDataAvalable(NetworkStream stream, ulong count)
        {
            List<byte> bytes = new List<byte>();
            ulong lengthReaded = 0;

            while (lengthReaded < count)
            {
                if (!stream.DataAvailable)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Thread.Sleep(1000);
                        if (stream.DataAvailable)
                            break;
                    }
                    if (!stream.DataAvailable)
                        break;
                }
                ulong countToRead = count;
                if (lengthReaded + countToRead > count)
                {
                    countToRead = count - lengthReaded;
                }
                byte[] readBytes = new byte[countToRead];
                var readCount = stream.Read(readBytes, 0, (int)countToRead);
                if (readCount <= 0)
                    throw new Exception("read zero buffer! client disconnected: " + readCount);
                lengthReaded += (ulong)readCount;
                bytes.AddRange(readBytes.ToList().GetRange(0, readCount));
            }
            return bytes.ToArray();
        }
#endif
        //public static byte[] DecodeMessage(byte[] buffer, int length)
        //{
        //    byte b = buffer[1];
        //    int dataLength = 0;
        //    int totalLength = 0;
        //    int keyIndex = 0;

        //    if (b - 128 <= 125)
        //    {
        //        dataLength = b - 128;
        //        keyIndex = 2;
        //        totalLength = dataLength + 6;
        //    }

        //    if (b - 128 == 126)
        //    {
        //        dataLength = BitConverter.ToInt16(new byte[] { buffer[3], buffer[2] }, 0);
        //        keyIndex = 4;
        //        totalLength = dataLength + 8;
        //    }

        //    if (b - 128 == 127)
        //    {
        //        dataLength = (int)BitConverter.ToInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
        //        keyIndex = 10;
        //        totalLength = dataLength + 14;
        //    }

        //    if (totalLength > length)
        //        throw new Exception("The buffer length is small than the data length");

        //    byte[] key = new byte[] { buffer[keyIndex], buffer[keyIndex + 1], buffer[keyIndex + 2], buffer[keyIndex + 3] };

        //    int dataIndex = keyIndex + 4;
        //    int count = 0;
        //    for (int i = dataIndex; i < totalLength; i++)
        //    {
        //        buffer[i] = (byte)(buffer[i] ^ key[count % 4]);
        //        count++;
        //    }

        //    return buffer.ToList().GetRange(dataIndex, dataLength).ToArray();// Encoding.ASCII.GetString(buffer, dataIndex, dataLength);
        //}
        //static byte[] DecodeMessage(byte[] byt;es)
        //{
        //    string incomingData = string.Empty;
        //    byte secondByte = bytes[1];
        //    int dataLength = secondByte & 127;
        //    int indexFirstMask = 2;
        //    if (dataLength == 126)
        //        indexFirstMask = 4;
        //    else if (dataLength == 127)
        //        indexFirstMask = 10;

        //    IEnumerable<byte> keys = bytes.Skip(indexFirstMask).Take(4);
        //    int indexFirstDataByte = indexFirstMask + 4;

        //    byte[] decoded = new byte[bytes.Length - indexFirstDataByte];
        //    for (int i = indexFirstDataByte, j = 0; i < bytes.Length; i++, j++)
        //    {
        //        decoded[j] = (byte)(bytes[i] ^ keys.ElementAt(j % 4));
        //    }

        //    return decoded;
        //}
        static byte[] DecodeMessage(byte[] bytes)
        {
            List<byte> ret = new List<byte>();
            int offset = 0;
            while (offset + 6 < bytes.Length)
            {
                // format: 0==ascii/binary 1=length-0x80, byte 2,3,4,5=key, 6+len=message, repeat with offset for next...
                int len = bytes[offset + 1] - 0x80;

                if (len <= 125)
                {

                    //String data = Encoding.UTF8.GetString(bytes);
                    //Debug.Log("len=" + len + "bytes[" + bytes.Length + "]=" + ByteArrayToString(bytes) + " data[" + data.Length + "]=" + data);
                    //Debug.Log("len=" + len + " offset=" + offset);
                    Byte[] key = new Byte[] { bytes[offset + 2], bytes[offset + 3], bytes[offset + 4], bytes[offset + 5] };
                    Byte[] decoded = new Byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        int realPos = offset + 6 + i;
                        decoded[i] = (Byte)(bytes[realPos] ^ key[i % 4]);
                    }
                    offset += 6 + len;
                    ret.AddRange(decoded);
                }
                else
                {
                    int a = bytes[offset + 2];
                    int b = bytes[offset + 3];
                    len = (a << 8) + b;
                    //Debug.Log("Length of ws: " + len);

                    Byte[] key = new Byte[] { bytes[offset + 4], bytes[offset + 5], bytes[offset + 6], bytes[offset + 7] };
                    Byte[] decoded = new Byte[len];
                    for (int i = 0; i < len; i++)
                    {
                        int realPos = offset + 8 + i;
                        decoded[i] = (Byte)(bytes[realPos] ^ key[i % 4]);
                    }

                    offset += 8 + len;
                    ret.AddRange(decoded);
                }
            }
            return ret.ToArray();
        }
        //static byte[] DecodeMessage(byte[] buffer)
        //{
        //    byte b = buffer[1];
        //    int dataLength = 0;
        //    int totalLength = 0;
        //    int keyIndex = 0;
        //    var takeLength = b - 128;
        //    if (takeLength == 0)
        //    {
        //        return new byte[] { 1 };
        //    }
        //    else if (takeLength <= 125)
        //    {
        //        dataLength = b - 128;
        //        keyIndex = 2;
        //        totalLength = dataLength + 6;
        //    }
        //    else if (takeLength == 126)
        //    {
        //        dataLength = BitConverter.ToInt16(new byte[] { buffer[3], buffer[2] }, 0);
        //        keyIndex = 4;
        //        totalLength = dataLength + 8;
        //    }
        //    else if (takeLength == 127)
        //    {
        //        dataLength = (int)BitConverter.ToInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
        //        keyIndex = 10;
        //        totalLength = dataLength + 14;
        //    }
        //    //if (totalLength > buffer.Length)
        //    //    throw new Exception("The buffer length is small than the data length");

        //    byte[] key = new byte[] { buffer[keyIndex], buffer[keyIndex + 1], buffer[keyIndex + 2], buffer[keyIndex + 3] };

        //    int dataIndex = keyIndex + 4;
        //    int count = 0;
        //    for (int i = dataIndex; i < totalLength; i++)
        //    {
        //        buffer[i] = (byte)(buffer[i] ^ key[count % 4]);
        //        count++;
        //    }

        //    return buffer.ToList().GetRange(dataIndex, dataLength).ToArray();// Encoding.ASCII.GetString(, , );
        //}
    }
}
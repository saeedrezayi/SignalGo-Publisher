﻿// AUTO GENERATED
//     This code was generated by signalgo add refenreces.
//     Runtime Version : 4.6.3.9
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
//     to download signalgo vsix for visual studio go https://marketplace.visualstudio.com/items?itemName=AliVisualStudio.SignalGoExtension
//     support and use signalgo go https://github.com/SignalGo/SignalGo-full-net
// AUTO GENERATED
using SignalGo.Shared.DataTypes;
using System.Threading.Tasks;
using SignalGo.Shared.Models;
using SignalGo.Publisher.Shared.Models;
using ServerManagerService.Interfaces;
using ServerManagerService.ServerServices;
using ServerManagerService.HttpServices;
using ServerManagerService.ClientServices;
using System;
namespace ServerManagerService.Interfaces
{

    public partial interface IServerManagerServiceSync
    {
        bool StopServer(System.Guid serverKey);
        bool StartServer(System.Guid serverKey);
        string CallClientService(string message);
        string SayHello(string name);
    }
    public partial interface IServerManagerServiceAsync
    {
        Task<bool> StopServerAsync(System.Guid serverKey);
        Task<bool> StartServerAsync(System.Guid serverKey);
        Task<string> CallClientServiceAsync(string message);
        Task<string> SayHelloAsync(string name);
    }
    [ServiceContract("servermanagerserverservice", ServiceType.ServerService, InstanceType.SingleInstance)]
    public partial interface IServerManagerService: IServerManagerServiceAsync, IServerManagerServiceSync
    {
    }
    public partial interface IServerManagerStreamServiceSync
    {
        string UploadData(SignalGo.Shared.Models.StreamInfo streamInfo, SignalGo.Publisher.Shared.Models.ServiceContract serviceContract);
        SignalGo.Shared.Models.StreamInfo DownloadFileData(string filePath, System.Guid serviceKey);
        bool SaveFileData(SignalGo.Shared.Models.StreamInfo<string> stream, System.Guid serviceKey);
        SignalGo.Shared.Models.StreamInfo CaptureApplicationProcess(System.Guid serviceKey);
    }
    public partial interface IServerManagerStreamServiceAsync
    {
        Task<string> UploadDataAsync(SignalGo.Shared.Models.StreamInfo streamInfo, SignalGo.Publisher.Shared.Models.ServiceContract serviceContract);
        Task<SignalGo.Shared.Models.StreamInfo> DownloadFileDataAsync(string filePath, System.Guid serviceKey);
        Task<bool> SaveFileDataAsync(SignalGo.Shared.Models.StreamInfo<string> stream, System.Guid serviceKey);
        Task<SignalGo.Shared.Models.StreamInfo> CaptureApplicationProcessAsync(System.Guid serviceKey);
    }
    [ServiceContract("serverstreammanagerstreamservice", ServiceType.StreamService, InstanceType.SingleInstance)]
    public partial interface IServerManagerStreamService: IServerManagerStreamServiceAsync, IServerManagerStreamServiceSync
    {
    }
    public partial interface IFileManagerServiceSync
    {
        System.Collections.Generic.List<string> GetTextFiles(System.Guid serviceKey);
    }
    public partial interface IFileManagerServiceAsync
    {
        Task<System.Collections.Generic.List<string>> GetTextFilesAsync(System.Guid serviceKey);
    }
    [ServiceContract("filemanagerserverservice", ServiceType.ServerService, InstanceType.SingleInstance)]
    public partial interface IFileManagerService: IFileManagerServiceAsync, IFileManagerServiceSync
    {
    }
}

namespace ServerManagerService.ServerServices
{
    [ServiceContract("servermanagerserverservice",ServiceType.ServerService, InstanceType.SingleInstance)]
    public partial class ServerManagerService : IServerManagerService
    {
        private SignalGo.Client.ClientProvider CurrentProvider { get; set; }
        string ServiceName { get; set; }
        public ServerManagerService(SignalGo.Client.ClientProvider clientProvider)
        {
            CurrentProvider = clientProvider;
            ServiceName = this.GetType().GetServerServiceName(true);
        }
        public bool StopServer(System.Guid serverKey)
        {
                return  SignalGo.Client.ClientManager.ConnectorExtensions.SendDataSync<bool>(CurrentProvider, ServiceName,"StopServer", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serverKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serverKey) },
                });
        }
        public Task<bool> StopServerAsync(System.Guid serverKey)
        {
                return SignalGo.Client.ClientManager.ConnectorExtensions.SendDataAsync<bool>(CurrentProvider, ServiceName,"StopServer", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serverKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serverKey) },
                });
        }
        public bool StartServer(System.Guid serverKey)
        {
                return  SignalGo.Client.ClientManager.ConnectorExtensions.SendDataSync<bool>(CurrentProvider, ServiceName,"StartServer", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serverKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serverKey) },
                });
        }
        public Task<bool> StartServerAsync(System.Guid serverKey)
        {
                return SignalGo.Client.ClientManager.ConnectorExtensions.SendDataAsync<bool>(CurrentProvider, ServiceName,"StartServer", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serverKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serverKey) },
                });
        }
        public string CallClientService(string message)
        {
                return  SignalGo.Client.ClientManager.ConnectorExtensions.SendDataSync<string>(CurrentProvider, ServiceName,"CallClientService", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(message),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(message) },
                });
        }
        public Task<string> CallClientServiceAsync(string message)
        {
                return SignalGo.Client.ClientManager.ConnectorExtensions.SendDataAsync<string>(CurrentProvider, ServiceName,"CallClientService", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(message),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(message) },
                });
        }
        public string SayHello(string name)
        {
                return  SignalGo.Client.ClientManager.ConnectorExtensions.SendDataSync<string>(CurrentProvider, ServiceName,"SayHello", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(name),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(name) },
                });
        }
        public Task<string> SayHelloAsync(string name)
        {
                return SignalGo.Client.ClientManager.ConnectorExtensions.SendDataAsync<string>(CurrentProvider, ServiceName,"SayHello", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(name),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(name) },
                });
        }
    }
    [ServiceContract("filemanagerserverservice",ServiceType.ServerService, InstanceType.SingleInstance)]
    public partial class FileManagerService : IFileManagerService
    {
        private SignalGo.Client.ClientProvider CurrentProvider { get; set; }
        string ServiceName { get; set; }
        public FileManagerService(SignalGo.Client.ClientProvider clientProvider)
        {
            CurrentProvider = clientProvider;
            ServiceName = this.GetType().GetServerServiceName(true);
        }
        public System.Collections.Generic.List<string> GetTextFiles(System.Guid serviceKey)
        {
                return  SignalGo.Client.ClientManager.ConnectorExtensions.SendDataSync<System.Collections.Generic.List<string>>(CurrentProvider, ServiceName,"GetTextFiles", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                });
        }
        public Task<System.Collections.Generic.List<string>> GetTextFilesAsync(System.Guid serviceKey)
        {
                return SignalGo.Client.ClientManager.ConnectorExtensions.SendDataAsync<System.Collections.Generic.List<string>>(CurrentProvider, ServiceName,"GetTextFiles", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                });
        }
    }
}

namespace ServerManagerService.StreamServices
{
    [ServiceContract("serverstreammanagerstreamservice",ServiceType.StreamService, InstanceType.SingleInstance)]
    public partial class ServerManagerStreamService : IServerManagerStreamService
    {
        public string ServerAddress { get; set; }
        public int? Port { get; set; }
        private string ServiceName { get; set; }

        private SignalGo.Client.ClientProvider CurrentProvider { get; set; }
        public ServerManagerStreamService(SignalGo.Client.ClientProvider clientProvider = null)
        {
            CurrentProvider = clientProvider;
            ServiceName = GetType().GetServerServiceName(true);
        }

        public ServerManagerStreamService(string serverAddress = null, int? port = null, SignalGo.Client.ClientProvider clientProvider = null)
        {
            ServerAddress = serverAddress;
            Port = port;
            CurrentProvider = clientProvider;
            ServiceName = GetType().GetServerServiceName(true);
        }
        public string UploadData(SignalGo.Shared.Models.StreamInfo streamInfo, SignalGo.Publisher.Shared.Models.ServiceContract serviceContract)
        {
                return  SignalGo.Client.ClientProvider.UploadStreamSync<string>(CurrentProvider, ServerAddress, Port, ServiceName ,"UploadData", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(streamInfo),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(streamInfo) },
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceContract),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceContract) },
                }, streamInfo);
        }
        public Task<string> UploadDataAsync(SignalGo.Shared.Models.StreamInfo streamInfo, SignalGo.Publisher.Shared.Models.ServiceContract serviceContract)
        {
                return SignalGo.Client.ClientProvider.UploadStreamAsync<string>(CurrentProvider, ServerAddress, Port, ServiceName ,"UploadData", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(streamInfo),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(streamInfo) },
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceContract),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceContract) },
                }, streamInfo);
        }
        public SignalGo.Shared.Models.StreamInfo DownloadFileData(string filePath, System.Guid serviceKey)
        {
                return  SignalGo.Client.ClientProvider.UploadStreamSync<SignalGo.Shared.Models.StreamInfo>(CurrentProvider, ServerAddress, Port, ServiceName ,"DownloadFileData", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(filePath),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(filePath) },
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                }, null);
        }
        public Task<SignalGo.Shared.Models.StreamInfo> DownloadFileDataAsync(string filePath, System.Guid serviceKey)
        {
                return SignalGo.Client.ClientProvider.UploadStreamAsync<SignalGo.Shared.Models.StreamInfo>(CurrentProvider, ServerAddress, Port, ServiceName ,"DownloadFileData", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(filePath),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(filePath) },
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                }, null);
        }
        public bool SaveFileData(SignalGo.Shared.Models.StreamInfo<string> stream, System.Guid serviceKey)
        {
                return  SignalGo.Client.ClientProvider.UploadStreamSync<bool>(CurrentProvider, ServerAddress, Port, ServiceName ,"SaveFileData", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(stream),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(stream) },
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                }, stream);
        }
        public Task<bool> SaveFileDataAsync(SignalGo.Shared.Models.StreamInfo<string> stream, System.Guid serviceKey)
        {
                return SignalGo.Client.ClientProvider.UploadStreamAsync<bool>(CurrentProvider, ServerAddress, Port, ServiceName ,"SaveFileData", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(stream),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(stream) },
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                }, stream);
        }
        public SignalGo.Shared.Models.StreamInfo CaptureApplicationProcess(System.Guid serviceKey)
        {
                return  SignalGo.Client.ClientProvider.UploadStreamSync<SignalGo.Shared.Models.StreamInfo>(CurrentProvider, ServerAddress, Port, ServiceName ,"CaptureApplicationProcess", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                }, null);
        }
        public Task<SignalGo.Shared.Models.StreamInfo> CaptureApplicationProcessAsync(System.Guid serviceKey)
        {
                return SignalGo.Client.ClientProvider.UploadStreamAsync<SignalGo.Shared.Models.StreamInfo>(CurrentProvider, ServerAddress, Port, ServiceName ,"CaptureApplicationProcess", new SignalGo.Shared.Models.ParameterInfo[]
                {
                         new  SignalGo.Shared.Models.ParameterInfo() { Name = nameof(serviceKey),Value = SignalGo.Client.ClientSerializationHelper.SerializeObject(serviceKey) },
                }, null);
        }
    }
}

namespace ServerManagerService.OneWayServices
{
}

namespace ServerManagerService.HttpServices
{
}

namespace ServerManagerService.ClientServices
{
}


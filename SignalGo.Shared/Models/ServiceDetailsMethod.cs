﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SignalGo.Shared.Models
{
    /// <summary>
    /// method of service
    /// </summary>
    public class ServiceDetailsMethod
    {
        /// <summary>
        /// id of class
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// name of method
        /// </summary>
        public string MethodName { get; set; }
        /// <summary>
        /// comment of class
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// comment of return type
        /// </summary>
        public string ReturnComment { get; set; }
        /// <summary>
        /// comment of exceptions
        /// </summary>
        public string ExceptionsComment { get; set; }
        /// <summary>
        /// return type
        /// </summary>
        public string ReturnType { get; set; }
        /// <summary>
        /// test example to call thi method
        /// </summary>
        public string TestExample { get; set; }
        /// <summary>
        /// requests of method
        /// </summary>
#if (!NET35)
        public ObservableCollection<ServiceDetailsRequestInfo> Requests { get; set; }
#endif
        /// <summary>
        /// if item is exanded from treeview
        /// </summary>
        public bool IsExpanded { get; set; }
        /// <summary>
        /// if item is selected from treeview
        /// </summary>
        public bool IsSelected { get; set; }
#if (!NET35)
        public ServiceDetailsMethod Clone()
        {
            return new ServiceDetailsMethod() { Id = Id, Comment = Comment, ExceptionsComment = ExceptionsComment, MethodName = MethodName, Requests = new ObservableCollection<ServiceDetailsRequestInfo>(), ReturnComment = ReturnComment, ReturnType = ReturnType, TestExample = TestExample, IsSelected = IsSelected, IsExpanded = IsExpanded };
        }
#endif
    }

    public class ServiceDetailsRequestInfo
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// list of parameters
        /// </summary>
        public List<ServiceDetailsParameterInfo> Parameters { get; set; }

        public ServiceDetailsRequestInfo Clone()
        {
            return new ServiceDetailsRequestInfo() { Name = Name, Parameters = new List<ServiceDetailsParameterInfo>() };
        }
    }

    public class MethodParameterDetails
    {
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public int ParameterIndex { get; set; }
        public int ParametersCount { get; set; }
        public bool IsFull { get; set; }
    }
}
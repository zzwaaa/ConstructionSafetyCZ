using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.ViewModels
{
    /// <summary>
    /// 返回的数据对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseViewModel<T>
    {

        public int Count { get; private set; }
        public int Code { get; private set; }
        public string Message { get; private set; }
        //public string Msg { get; private set; }
        public T Data { get; private set; }
        public string Token { get; set; }

        public static ResponseViewModel<T> Create(int code, string message)
        {
            ResponseViewModel<T> entity = new ResponseViewModel<T>
            {
                Code = code,
                Message = message
            };
            return entity;
        }

        public static ResponseViewModel<T> Create(Status status, string message)
        {
            return Create((int)status, message);
        }

        public static ResponseViewModel<T> Create(int code, string message, T data)
        {
            var entity = Create(code, message);
            entity.Data = data;
            return entity;
        }

        public static ResponseViewModel<T> Create(Status status, string message, T data)
        {
            return Create((int)status, message, data);
        }

        public static ResponseViewModel<T> Create(int code, string message, T data, int totalCount)
        {
            var entity = Create(code, message, data);
            entity.Count = totalCount;
            return entity;

        }

        public static ResponseViewModel<T> Create(Status status, string message, T data, int totalCount)
        {
            return Create((int)status, message, data, totalCount);
        }

        public static ResponseViewModel<T> Create(int code, string message, T data, int totalCount, string token)
        {
            var entity = Create(code, message, data, totalCount);
            entity.Token = token;
            return entity;
        }

        public static ResponseViewModel<T> Create(Status status, string message, T data, int totalCount, string token)
        {
            return Create((int)status, message, data, totalCount, token);
        }

    }
    /// <summary>
    /// 状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 访问成功
        /// </summary>
        SUCCESS,
        /// <summary>
        /// 访问失败
        /// </summary>
        FAIL,
        /// <summary>
        /// 访问错误
        /// </summary>
        ERROR,
        /// <summary>
        /// 非法访问，警告
        /// </summary>
        WARN
      
    }

    /// <summary>
    /// 状态描述
    /// </summary>
    public static class Message
    {
        /// <summary>
        /// 访问成功
        /// </summary>
        public const string SUCCESS = "访问成功";
        /// <summary>
        /// 访问失败
        /// </summary>
        public const string FAIL = "访问失败";
        /// <summary>
        /// 内部错误
        /// </summary>
        public const string ERROR = "系统异常，请稍后重试";

        /// <summary>
        /// 非法访问
        /// </summary>
        public const string WARN = "非法访问";

    }
}


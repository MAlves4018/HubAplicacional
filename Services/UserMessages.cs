using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace WebApp.Services
{
    public abstract class IUserMessages
    {
        public enum ErrorCode : int
        {
            SUCCESS = 1,
            DANGER = 2,
            WARNING = 3,
            INFO = 4
        }

        public const int DefaultTimeout = 5000;
        public abstract bool AddUserMessage(string title, string message, ErrorCode errorCode, int timeout);
        public abstract bool AddUserMessageOnce(string msgId, string title, string message, ErrorCode errorCode, int timeout);
        public abstract string GetUserMessages();
    }

    public class UserMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public IUserMessages.ErrorCode Error { get; set; }

        public int Timeout { get; set; }
    }

    public class UserMessages : IUserMessages
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserMessages(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            
        }

        public override bool AddUserMessageOnce(string msgId, string title, string message, IUserMessages.ErrorCode errorCode = IUserMessages.ErrorCode.INFO, int timeout = IUserMessages.DefaultTimeout)
        {
            try
            {
                string toShow = _httpContextAccessor.HttpContext.Session.GetString("UserMessagesIds") ?? "[]";
                List<string> errorMessages = JsonConvert.DeserializeObject<List<string>>(toShow);
                if (!errorMessages.Contains(msgId) && AddUserMessage(title, message, errorCode, timeout))
                {
                    errorMessages.Add(msgId);
                    toShow = JsonConvert.SerializeObject(errorMessages);
                    _httpContextAccessor.HttpContext.Session.SetString("UserMessagesIds", toShow);
                    return true;
                }
            }
            catch (Exception)
            { }
            return false;
        }

        public override bool AddUserMessage(string title, string message, IUserMessages.ErrorCode errorCode = IUserMessages.ErrorCode.INFO, int timeout = IUserMessages.DefaultTimeout)
        {
            try
            {
                string toShow = _httpContextAccessor.HttpContext.Session.GetString("UserMessages") ?? "[]";
                List<UserMessage> errorMessages = JsonConvert.DeserializeObject<List<UserMessage>>(toShow);
                errorMessages.Add(new UserMessage() { Title = title, Message = message, Error = errorCode, Timeout = timeout });
                toShow = JsonConvert.SerializeObject(errorMessages);
                _httpContextAccessor.HttpContext.Session.SetString("UserMessages", toShow);
                return true;
            }
            catch (Exception) { }
            return false;
        }

        public override string GetUserMessages()
        {
            string toShow = _httpContextAccessor.HttpContext.Session.GetString("UserMessages");
            _httpContextAccessor.HttpContext.Session.SetString("UserMessages", "[]");
            return toShow ?? "[]";
        }

    }
}

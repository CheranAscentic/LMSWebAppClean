using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSWebAppClean.Application.DTO
{
    public class StandardResponseObject<T> where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public StandardResponseObject(bool Success, string? Message, T? Data)
        {
            Success = Success;
            Message = Message;
            Data = Data;
        }
    }
}

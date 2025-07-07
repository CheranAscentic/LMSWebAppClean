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
        public int Status { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }

        public StandardResponseObject()
        {
        }

        public StandardResponseObject(bool success, string? message, int status, T? data, string? error = null)
        {
            Success = success;
            Message = message;
            Status = status;
            Data = data;
            Error = error;
        }

        // Factory methods for common responses
        public static StandardResponseObject<T> Ok(T? data, string? message = null)
        {
            return new StandardResponseObject<T>(true, message ?? "Success", 200, data);
        }

        public static StandardResponseObject<T> Created(T? data, string? message = null)
        {
            return new StandardResponseObject<T>(true, message ?? "Created", 201, data);
        }

        public static StandardResponseObject<T> BadRequest(string? error = null, string? message = null)
        {
            return new StandardResponseObject<T>(false, message ?? "Bad Request", 400, null, error);
        }

        public static StandardResponseObject<T> NotFound(string? error = null, string? message = null)
        {
            return new StandardResponseObject<T>(false, message ?? "Not Found", 404, null, error);
        }

        public static StandardResponseObject<T> InternalError(string? error = null, string? message = null)
        {
            return new StandardResponseObject<T>(false, message ?? "Internal Server Error", 500, null, error);
        }
    }
}

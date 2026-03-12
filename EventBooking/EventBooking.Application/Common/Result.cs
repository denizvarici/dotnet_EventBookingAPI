using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EventBooking.Application.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }



        //helper method for success and failure object results
        public static Result<T> Success(T data, string message = "Process succeeded.")
            => new() { IsSuccess = true, Data = data, Message = message };

        public static Result<T> Failure(T data,string error)
            => new() { IsSuccess = false,Data = data, Errors = new List<string> { error }, Message = "An error occurred." };

        public static Result<T> Failure(T data,List<string> errors)
            => new() { IsSuccess = false,Data = data, Errors = errors, Message = "Multiple errors occurred." };
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EventBooking.Application.Common
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public IEnumerable<string> Errors { get; set; }


        //helper method for success
        public static Result<T> SuccessResult(T data, string message = "Process succeeded.")
            => new() { Success = true, Data = data, Message = message };

        public static Result<T> FailureResult(T data, string message = "An error occured.")
        {
            return new() { Success = false, Data = data, Message = message };
        }
        public static Result<T> FailureResult(string error, string message = "An error occured.")
        {
            List<string> errors = new();
            errors.Add(error);
            return new() { Success = false, Errors = errors, Message = message };
        }
        //helper method for failures
        public static Result<T> FailureResult(IEnumerable<string> errors, string message = "An error occured.")
            => new() { Success = false, Errors = errors, Message = message };
    }
}

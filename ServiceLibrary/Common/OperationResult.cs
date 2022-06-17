using Models;
using System;


namespace Common
{
    public class OperationResult
    {

        public bool Success { get; private set; }
        public string Message { get; private set; }

        public Exception Exception { get; private set; }

        public static OperationResult BuildSuccessResult(string Message)
        {
            return new OperationResult { Success = true, Message = Message };
        }

        public static OperationResult BuildFailure(string Message)
        {
            return new OperationResult { Success = false, Message = Message };

        }
        public static OperationResult BuildFailure(Exception ex)
        {
            return new OperationResult { Success = false, Exception = ex };
        }

        public static OperationResult BuildFailure(Exception ex, string Message)
        {
            return new OperationResult { Success = false, Exception = ex, Message = Message };
        }

        public ServiceDto ToServiceDto()
        {
            return new ServiceDto() { Message = Message, Status = (byte)(Success ? 1 : 0) };
        } 
    }
    public class OperationResult<TResult>
    {
        public string Message { get; private set; }
        public TResult Result { get; private set; }

        public bool Success { get; private set; }
        public Exception Exception { get; private set; }

        public static OperationResult<TResult> BuildSuccessResult(TResult result, string Message = "")
        {
            return new OperationResult<TResult> { Success = true, Result = result, Message = Message };

        }

        public static OperationResult<TResult> BuildFailure(string Message)
        {
            return new OperationResult<TResult> { Success = false, Message = Message };

        }
        public static OperationResult<TResult> BuildFailure(Exception ex)
        {
            return new OperationResult<TResult> { Success = false, Exception = ex };
        }

        public static OperationResult<TResult> BuildFailure(Exception ex, string Message)
        {
            return new OperationResult<TResult> { Success = false, Exception = ex, Message = Message };
        }



        public ServiceDto<TResult> ToServiceDto()
        {
            return new ServiceDto<TResult>() { Message = Message, Data = Result, Status = (byte)(Success ? 1 : 0) };
        }
    }
}

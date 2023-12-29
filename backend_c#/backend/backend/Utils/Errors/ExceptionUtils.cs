using System;
using backend.Exceptions;

namespace backend.Utils.Errors;

public class ExceptionUtils{
    public static ExceptionResponseModel FormatExceptionResponse(Exception ex){
        return  new ExceptionResponseModel(){
                Error = new ExceptionDetails(){
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message,
                    Type = ex.GetType().ToString()
                }
        };
    }
}
﻿namespace backend.Utils.Errors
{
    public class ErrorUtils
    {

        public static object FormatError(Exception ex)
        {

            return new
            {
                error = ex.Message
            };
        }

    }
}
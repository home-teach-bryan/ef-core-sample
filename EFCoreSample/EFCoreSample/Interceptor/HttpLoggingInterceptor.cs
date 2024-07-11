﻿using Microsoft.AspNetCore.HttpLogging;

namespace EFCoreSample.Interceptor;

public class HttpLoggingInterceptor : IHttpLoggingInterceptor
{
    public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
    {
        if (logContext.HttpContext.Request.Path.Value.Contains("/api/Token") && logContext.HttpContext.Request.Method == "POST")
        {
            logContext.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath | HttpLoggingFields.ResponseStatusCode;
        }

        return ValueTask.CompletedTask;
    }

    public  ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext)
    {
        return ValueTask.CompletedTask;
    }
}
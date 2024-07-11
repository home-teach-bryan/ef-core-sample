﻿using EFCoreSample.Extension;
using EFCoreSample.Models.Enum;
using EFCoreSample.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EFCoreSample.ActionFilter;
public class ApiResponseActionFilter : IResultFilter
{
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && context.ModelState.IsValid)
        {
            var apiResponseStatus = objectResult.StatusCode == StatusCodes.Status200OK
                ? ApiResponseStatus.Success
                : ApiResponseStatus.Fail;
            
            var data = objectResult.StatusCode == StatusCodes.Status200OK ? objectResult.Value : null;

            var error = objectResult.StatusCode == StatusCodes.Status400BadRequest
                ? new List<string> { objectResult.Value.ToString() }
                : null;
            var apiResponse = new ApiResponse<object>
            {
                Status = apiResponseStatus,
                Message = apiResponseStatus.GetDescription(),
                Data = data,
                Errors = error
            };
            var result = new ObjectResult(apiResponse)
            {
                StatusCode = objectResult.StatusCode
            };
            context.Result = result;
        }
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
        return;
    }
}
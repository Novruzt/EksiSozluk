using EksiSozluk.Common.Infrastructure.Exceptions;
using EksiSozluk.Common.Infrastructure.Results;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace EksiSozluk.Api.WebApi.Infrastructure.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app,
                                                                 bool includeExceptionDetails = false,
                                                                 bool useDefaultHandlingResponse = true,
                                                                 Func<HttpContext, Exception, Task> handleException = null)
    {

        app.UseExceptionHandler(options => {
            options.Run(context =>
            {
                IExceptionHandlerFeature exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

                if (!useDefaultHandlingResponse && handleException == null)
                    throw new ArgumentNullException(nameof(handleException), $"{nameof(handleException)} cannot be null when {nameof(useDefaultHandlingResponse)} is false");

                if (!includeExceptionDetails && handleException != null)
                    return handleException(context, exceptionObject.Error);


                return DefaultHandleException(context, exceptionObject.Error, includeExceptionDetails);
            });
        });


        return app;
    }



    private static async Task DefaultHandleException(HttpContext context, Exception exception, bool includeExceptionDetails)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string message = "Internal server error occured";

        if(exception is UnauthorizedAccessException)
           statusCode=HttpStatusCode.Unauthorized;

        if (exception is DatabaseValidationException)
        {
            statusCode=HttpStatusCode.BadRequest;
            ValidationResponseModel validationResponse = new ValidationResponseModel(exception.Message);
            await WriteResponse(context, statusCode, validationResponse);
            return;
        }

        var response = new
        {
            HttpStatusCode = (int)statusCode,
            Details = includeExceptionDetails ? exception.ToString() : message
        }; 

        await WriteResponse(context, statusCode, response); 

    }


    private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object responseObject)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode= (int)statusCode;

        await context.Response.WriteAsJsonAsync(responseObject);
    }
}

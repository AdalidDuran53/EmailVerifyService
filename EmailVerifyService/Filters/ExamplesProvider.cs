using Azure.Core;
using Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EmailVerifyService.Filters
{
    public class CustomResponseCreatedExample : IExamplesProvider<CustomResponse>
    {
        public CustomResponse GetExamples()
            => new CustomResponse(statusCode: StatusCodes.Status201Created, message: "send Verification Code successfully.", token: Guid.Empty);
    }

    public class CustomResponseBadRequestExample : IExamplesProvider<ErrorResponse>
    {
        public ErrorResponse GetExamples()
            => new ErrorResponse { StatusCode = StatusCodes.Status400BadRequest, Code = "EVS-CODE-ERROR", Message = "verify code invalid.", Details = "verify code invalid." };
    }

    public class CustomResponseOKExample : IExamplesProvider<CustomResponse>
    {
        public CustomResponse GetExamples()
            => new CustomResponse(statusCode: StatusCodes.Status200OK, message: "validated verify code successfully.", token: Guid.Empty);
    }

    public class RequestVerifyCodeExample : IExamplesProvider<object>
    {
        public object GetExamples()
            => new
            {
                version = "1.0",
                emailAddress = "user@example.com",
                appToken = Guid.NewGuid(),
                AuthenticationKey = Guid.NewGuid()
            };
    }


}

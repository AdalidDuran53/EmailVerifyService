using Domain;
using EmailVerifyService.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmailVerifyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class EmailVerifyControllerBase : ControllerBase
    {

        [HttpPost]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/EmailVerify/")]
        [SwaggerOperation(OperationId = "RequestVerifyCode")]
        [SwaggerResponse(statusCode: StatusCodes.Status201Created, type: typeof(CustomResponse), description: "Ok")]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CustomResponseOKExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse), description: "Bab Request")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CustomResponseBadRequestExample))]
        public abstract Task<IActionResult> RequestVerifyCode(
            [FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")][DefaultValue("0.1")] string version, 
            [Required, EmailAddress] string emailAddress, 
            Guid? appToken);

        [HttpPost]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/EmailVerify/")]
        [SwaggerOperation(OperationId = "ValidateVerifyCode")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(CustomResponse), description: "Ok")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomResponseOKExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse), description: "Bab Request")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CustomResponseBadRequestExample))]
        public abstract Task<IActionResult> ValidateVerifyCode(
            [FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")][DefaultValue("0.1")] string version, 
            [Required, EmailAddress] string emailAddress, 
            [Required] Guid token, [Required] string verifyCode );

    }
}

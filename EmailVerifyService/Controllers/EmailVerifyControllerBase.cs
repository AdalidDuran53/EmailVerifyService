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
        [SwaggerResponse(statusCode: StatusCodes.Status201Created, type: typeof(ActionResult), description: "Created")]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CustomResponseCreatedExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CustomResponseBadRequestExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ActionResult), description: "Unauthorized")][SwaggerRequestExample(typeof(object), typeof(RequestVerifyCodeExample))]
        public abstract Task<IActionResult> RequestVerifyCode(
            [FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")][DefaultValue("0.1")] string version, 
            [Required, EmailAddress] string emailAddress, 
            Guid? appToken);

        [HttpPost]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/EmailVerify/")]
        [SwaggerOperation(OperationId = "ValidateVerifyCode")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomResponseOKExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CustomResponseBadRequestExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status401Unauthorized, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> ValidateVerifyCode(
            [FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")][DefaultValue("0.1")] string version, 
            [Required, EmailAddress] string emailAddress, 
            [Required] Guid token, [Required] string verifyCode );

    }
}

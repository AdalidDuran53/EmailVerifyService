using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;
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
        [SwaggerResponse(statusCode: 201, type: typeof(ActionResult), description: "Created")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> RequestVerifyCode([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required] Guid appToken, [Required, EmailAddress] string emailAddress);

        [HttpPost]
        [EnableRateLimiting("IpPolicy")]
        [Route("~/{version}/EmailVerify/")]
        [SwaggerOperation(OperationId = "ValidateVerifyCode")]
        [SwaggerResponse(statusCode: 200, type: typeof(ActionResult), description: "Ok")]
        [SwaggerResponse(statusCode: 400, type: typeof(ActionResult), description: "Bab Request")]
        [SwaggerResponse(statusCode: 401, type: typeof(ActionResult), description: "Unauthorized")]
        public abstract Task<IActionResult> ValidateVerifyCode([FromRoute][Required][RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$")] string version, [Required, EmailAddress] string emailAddress, [Required] Guid Token, [Required] string verifyCode );

    }
}

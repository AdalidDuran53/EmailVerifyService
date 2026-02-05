using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ExceptionManagement;
using EmailVerifyService.Business;
using EmailVerifyService.Models;
using System.ComponentModel.DataAnnotations;

namespace EmailVerifyService.Controllers.Implementation
{
    [ApiVersion("0.1")]
    [ApiController]
    [Authorize]
    public class EmailVerifyApi : EmailVerifyControllerBase
    {
        private readonly VerfyCodeFunctionality _verfyCodeFunctionality;
        private readonly ServiceBaseFunctionality _serviceBaseFunctionality;
        private readonly ErrorServiceModel _errorService = new ErrorServiceModel();
        public EmailVerifyApi(ServiceBaseFunctionality serviceBaseFunctionality, VerfyCodeFunctionality verfyCodeFunctionality)
        {
            _serviceBaseFunctionality = serviceBaseFunctionality;
            _errorService = new ErrorServiceModel();
            _verfyCodeFunctionality = verfyCodeFunctionality;
        }


        [HttpPost]
        [Route("~/{version::apiVersion}/EmailVerify/RequestVerifyCode")]
        public async override Task<IActionResult> RequestVerifyCode([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] Guid appToken, [Required, EmailAddress] string emailAddress)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "RequestVerifyCodeRequest", new object[] { "version: " + version, "emailAddress: " + emailAddress } } };
            try
            {
                // Call the implementation
                var result = await _verfyCodeFunctionality.RequestVerifyCode(emailAddress, appToken);
                // Log the response
                Dictionary<string, object> response = new Dictionary<string, object> { { "RequestVerifyCodeResponse", result } };
                // Log the operation
                await _serviceBaseFunctionality.LogOperation(request, response, result.Token);
                // return the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorRequestVerifyCodeResponse", ex } };
                await _serviceBaseFunctionality.LogOperation(request, response);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new { StatusCode = StatusCodes.Status400BadRequest, code = excep.ErrorCode, message = excep.Message, details = excep.Details });
            }
        }

        [HttpPost]
        [Route("~/{version::apiVersion}/EmailVerify/ValidateVerifyCode")]
        public async override Task<IActionResult> ValidateVerifyCode([FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, [Required] string emailAddress, [Required] Guid Token, [Required] string verifyCode)
        {
            throw new NotImplementedException();
        }
    }
}

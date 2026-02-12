using Domain;
using EmailVerifyService.Business;
using EmailVerifyService.Filters;
using EmailVerifyService.Models;
using ExceptionManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
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
        [SwaggerResponse(statusCode: StatusCodes.Status201Created, type: typeof(CustomResponse), description: "Ok")]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CustomResponseOKExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse), description: "Bab Request")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CustomResponseBadRequestExample))]
        public async override Task<IActionResult> RequestVerifyCode(
            [FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, 
            [Required, EmailAddress] string emailAddress, 
            Guid? appToken = null)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "RequestVerifyCodeRequest", new object[] { "version: " + version, "emailAddress: " + emailAddress } } };
            try
            {
                // Call the implementation
                var result = await _verfyCodeFunctionality.RequestVerifyCode(emailAddress, appToken?? Guid.Empty);
                // Log the response
                Dictionary<string, object> response = new Dictionary<string, object> { { "RequestVerifyCodeResponse", result } };
                // Log the operation
                await _serviceBaseFunctionality.LogOperation(request, response, result.Token);
                // return the result
                return this.Created(String.Empty, result);
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorRequestVerifyCodeResponse", ex } };
                await _serviceBaseFunctionality.LogOperation(request, response);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new ErrorResponse(statusCode: StatusCodes.Status400BadRequest, code: excep.ErrorCode, message: excep.Message, details: excep.Details));
            }
        }

        [HttpPost]
        [Route("~/{version::apiVersion}/EmailVerify/ValidateVerifyCode")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, type: typeof(CustomResponse), description: "Ok")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CustomResponseOKExample))]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, type: typeof(ErrorResponse), description: "Bab Request")]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CustomResponseBadRequestExample))]
        public async override Task<IActionResult> ValidateVerifyCode(
            [FromRoute, RegularExpression("^(?<major>[0-9]+).(?<major>[0-9]+)$"), Required] string version, 
            [Required] string emailAddress, [Required] Guid token, 
            [Required] string verifyCode)
        {
            // Log the request
            Dictionary<string, object> request = new Dictionary<string, object> { { "RequestValidateVerifyCodeRequest", new object[] { "version: " + version, "emailAddress: " + emailAddress } } };
            try
            {
                // Call the implementation
                var result = await _verfyCodeFunctionality.ValidateVerifyCode(emailAddress, token, verifyCode);
                // Log the response
                Dictionary<string, object> response = new Dictionary<string, object> { { "RequestValidateVerifyCodeResponse", result } };
                // Log the operation
                await _serviceBaseFunctionality.LogOperation(request, response, result.Token);
                // return the result
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception
                Dictionary<string, object> response = new Dictionary<string, object> { { "ErrorRequestValidateVerifyCodeResponse", ex } };
                await _serviceBaseFunctionality.LogOperation(request, response);
                // if the exception is an OperationException, return a bad request with the error details
                OperationException excep = ((OperationException)ex);
                return this.BadRequest(new ErrorResponse(statusCode: StatusCodes.Status400BadRequest, code: excep.ErrorCode, message: excep.Message, details: excep.Details));
            }
        }
    }
}

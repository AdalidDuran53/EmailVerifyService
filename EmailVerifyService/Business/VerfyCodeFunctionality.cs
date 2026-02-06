using Domain;
using ExceptionManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Runtime;
using VerifyStatusCodes = Domain.VerifyStatusCodes;

namespace EmailVerifyService.Business
{
    public class VerfyCodeFunctionality : FunctionalityBaseController
    {

        private readonly SmtpSettings _settings;
        private readonly RazorViewToStringRenderer _renderer;
        public VerfyCodeFunctionality(IOptions<SmtpSettings> settings,  RazorViewToStringRenderer renderer) { 
            _settings = settings.Value;
            _renderer = renderer;
        }
        public async Task<CustomResponse> RequestVerifyCode(string emailAddress, Guid? appToken)
        {
            try
            {

                var code = new Random().Next(100000, 999999).ToString();
                // build the Verification Code object
                VerifyCode newVerifyCode = new VerifyCode(code: code, token: Guid.NewGuid(), appToken: appToken?? Guid.Empty, emailAddress: emailAddress, operationDate: DateTime.Now, expirationDate: DateTime.Now.AddMinutes(15), verifyStatus: 1);
                // save the Verification Code object
                using (var context = new Models.EmailVerifyServiceDbContext())
                {
                    // check for duplicate Verification Code
                    var verifyCode = await context.VerifyCodes.FirstOrDefaultAsync(s => s.EmailAddress.Equals(newVerifyCode.EmailAddress) && s.AppToken.Equals(appToken)&& s.VerifyStatus == (int)VerifyStatusCodes.pending);
                    if (verifyCode != null)
                    {
                        verifyCode.VerifyStatus = (int)VerifyStatusCodes.Expired;
                        context.VerifyCodes.Update(verifyCode);
                        await context.SaveChangesAsync();
                    }
                    // map the Verification Code object to the entity model
                    var newCode = Mapster.TypeAdapter.Adapt<Models.VerifyCode>(newVerifyCode);
                    context.VerifyCodes.Add(newCode);
                    await context.SaveChangesAsync();
                    // send Verification Code
                    await SendVerificationCodeAsync(emailAddress, code);
                }
                // return the result
                var result = new CustomResponse(statusCode: StatusCodes.Status201Created, message: "send Verification Code successfully.", token: newVerifyCode.Token);
                return result;
            }
            catch (Exception ex)
            {
                // if the exception is an OperationException, rethrow it
                if (ex is OperationException)
                    throw ex;
                // otherwise, throw a general error
                var exception = this._errorService.GetError("EVS-GENERAL-ERROR");
                throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
            }
        }

        public async Task SendVerificationCodeAsync(string toEmail, string code)
        {

            var htmlBody = await _renderer.RenderViewToStringAsync(
                "EmailTemplates/VerificationCode",
                new VerificationModel { Code = code, ExpirationMinutes = 15 });

            using var smtp = new SmtpClient(_settings.Server, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.User, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            var mail = new MailMessage(_settings.From, toEmail)
            {
                Subject = "Código de verificación",
                Body = htmlBody,
                IsBodyHtml = true
            };

            await smtp.SendMailAsync(mail);
        }

        public async Task<CustomResponse> ValidateVerifyCode(string emailAddress, Guid Token, string verifyCode)
        {
            try
            {
                // build the Verification Code object
                VerifyCode newVerifyCode = new VerifyCode(code: verifyCode, token: Guid.NewGuid(), appToken: Guid.Empty, emailAddress: emailAddress, operationDate: DateTime.Now, expirationDate: DateTime.Now.AddMinutes(15), verifyStatus: 1);
                // save the Verification Code object
                using (var context = new Models.EmailVerifyServiceDbContext())
                {
                    // check for duplicate Verification Code
                    var verifyCodes = await context.VerifyCodes.FirstOrDefaultAsync(s => s.EmailAddress.Equals(newVerifyCode.EmailAddress) && s.Token.Equals(Token) && s.VerifyStatus == (int)VerifyStatusCodes.pending);
                    if (verifyCodes != null && verifyCodes.Code.Equals(verifyCode)
                        && verifyCodes.ExpirationDate > DateTime.Now)
                    {
                        verifyCodes.VerifyStatus = (int)VerifyStatusCodes.Verified;
                        context.VerifyCodes.Update(verifyCodes);
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        var exception = this._errorService.GetError("EVS-CODE-ERROR");
                        throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
                    }
                }
                // return the result
                var result = new CustomResponse(statusCode: StatusCodes.Status201Created, message: "validated verify code successfully.", token: newVerifyCode.Token);
                return result;
            }
            catch (Exception ex)
            {
                // if the exception is an OperationException, rethrow it
                if (ex is OperationException)
                    throw ex;
                // otherwise, throw a general error
                var exception = this._errorService.GetError("EVS-GENERAL-ERROR");
                throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
            }
        }
    }
}


public class SmtpSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string From { get; set; }
}

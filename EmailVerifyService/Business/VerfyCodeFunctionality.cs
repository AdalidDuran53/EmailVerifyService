using Domain;
using ExceptionManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Runtime;

namespace EmailVerifyService.Business
{
    public class VerfyCodeFunctionality : FunctionalityBaseController
    {

        private readonly SmtpSettings _settings;
        public VerfyCodeFunctionality(IOptions<SmtpSettings> settings) { 
            _settings = settings.Value;
        }
        public async Task<CustomResponse> RequestVerifyCode(string emailAddress, Guid appToken)
        {
            try
            {

                var code = new Random().Next(100000, 999999).ToString();
                // build the Verification Code object
                VerifyCode newVerifyCode = new VerifyCode(code: code, token: Guid.NewGuid(), appToken: appToken, emailAddress: emailAddress, operationDate: DateTime.Now, expirationDate: DateTime.Now.AddMinutes(15), verifyStatus: 1);
                // save the Verification Code object
                using (var context = new Models.EmailVerifyServiceDbContext())
                {
                    // check for duplicate Verification Code
                    var verifyCode = await context.VerifyCodes.FirstOrDefaultAsync(s => s.EmailAddress.Equals(newVerifyCode.EmailAddress) && s.AppToken.Equals(appToken));
                    if (verifyCode != null)
                    {
                        verifyCode.VerifyStatus = 3;
                        context.VerifyCodes.Update(verifyCode);
                        await context.SaveChangesAsync();
                    }
                    // map the Verification Code object to the entity model
                    var newCode = Mapster.TypeAdapter.Adapt<Models.VerifyCode>(newVerifyCode);
                    context.VerifyCodes.Add(newCode);
                    await context.SaveChangesAsync();
                    // send Verification Code
                    SendVerificationCodeAsync(emailAddress, code);
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
                var exception = this._errorService.GetError("OMS-GENERAL-ERROR");
                throw new OperationException(errorCode: exception.Code, message: exception.Message, details: exception.Details, new Guid());
            }
        }

        public async Task SendVerificationCodeAsync(string toEmail, string code)
        {
            var htmlBody = $@" <!DOCTYPE html> <html lang='es'> <head> <meta charset='UTF-8'> <title>Bienvenido - Verificación de correo</title> </head> <body style='font-family: Arial, sans-serif; background-color:#f4f4f4; padding:20px;'> <table width='100%' cellpadding='0' cellspacing='0' style='max-width:600px; margin:auto; background-color:#ffffff; border-radius:8px; box-shadow:0 2px 6px rgba(0,0,0,0.1);'> <tr> <td style='padding:20px; text-align:center;'> <h1 style='color:#2c3e50;'>¡Bienvenido!</h1> <p style='color:#555; font-size:16px;'> Gracias por registrarte en nuestra plataforma.<br/> Para completar tu proceso de verificación, utiliza el siguiente código: </p> <p style='font-size:28px; font-weight:bold; color:#2c3e50; background-color:#eaf2f8; padding:15px; border-radius:6px; display:inline-block; letter-spacing:4px;'> {code} </p> <p style='color:#777; font-size:14px; margin-top:20px;'> Este código expirará en <strong>15 minutos</strong>. </p> <p style='color:#999; font-size:12px; margin-top:30px;'> Si no solicitaste esta verificación, puedes ignorar este mensaje. </p> </td> </tr> </table> </body> </html>";

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

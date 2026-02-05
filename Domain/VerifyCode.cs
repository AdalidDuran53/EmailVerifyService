using ExceptionManagement;

namespace Domain;

public partial class VerifyCode : IValidation
{
    public VerifyCode()
    {
    }
    public VerifyCode(string code, Guid token, Guid appToken, string emailAddress, DateTime operationDate, DateTime expirationDate, int? verifyStatus = null, int? id = null)
    {
        this.Id = id;
        this.Code = code;
        this.Token = token;
        this.AppToken = appToken;
        this.EmailAddress = emailAddress;
        this.OperationDate = operationDate;
        this.ExpirationDate = expirationDate;
        this.VerifyStatus = verifyStatus;
    }
    public int? Id { get; set; }

    public string Code { get; set; } = null!;

    public Guid Token { get; set; }

    public Guid AppToken { get; set; }

    public string EmailAddress { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int? VerifyStatus { get; set; }


    public string Validate(string operationExceptionCode)
    {
        throw new NotImplementedException();
    }
}

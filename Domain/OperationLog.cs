using ExceptionManagement;

namespace Domain;

public partial class OperationLog : IValidation
{
    public OperationLog()
    {
    }
    public OperationLog(DateTime operationDate, string request, string response)
    {
        this.OperationDate = operationDate;
        this.Request = request;
        this.Response = response;
    }
    public int OperationId { get; set; }

    public DateTime? OperationDate { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }

    public string Validate(string operationExceptionCode)
    {
        throw new NotImplementedException();
    }
}

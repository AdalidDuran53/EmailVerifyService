using Microsoft.AspNetCore.Mvc;

namespace Domain;

public class CustomResponse : ActionResult
{
    public int StatusCode { get; }
    public string Message { get; }
    public Guid Token { get; }
    public object Data { get; }

    public CustomResponse(int statusCode, string message, Guid token, object data = null)
    {
        StatusCode = statusCode;
        this.Message = message;
        this.Token = token;
        this.Data = data;
    }
}

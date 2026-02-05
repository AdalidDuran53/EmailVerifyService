using System;
using System.Collections.Generic;

namespace EmailVerifyService.Models;

public partial class VerifyCode
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public Guid Token { get; set; }

    public Guid AppToken { get; set; }

    public string EmailAddress { get; set; } = null!;

    public DateTime OperationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int? VerifyStatus { get; set; }

    public virtual StatusCode? VerifyStatusNavigation { get; set; }
}

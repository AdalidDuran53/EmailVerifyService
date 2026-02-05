using System;
using System.Collections.Generic;

namespace EmailVerifyService.Models;

public partial class OperationLog
{
    public int OperationId { get; set; }

    public DateTime? OperationDate { get; set; }

    public string? Request { get; set; }

    public string? Response { get; set; }
}

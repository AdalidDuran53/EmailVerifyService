using System;
using System.Collections.Generic;

namespace EmailVerifyService.Models;

public partial class SessionLog
{
    public Guid SessionId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime InitSession { get; set; }

    public DateTime? EndSession { get; set; }

    public virtual ICollection<OperationLog> OperationLogs { get; set; } = new List<OperationLog>();

    public virtual User? User { get; set; }
}

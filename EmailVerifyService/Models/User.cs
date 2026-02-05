using System;
using System.Collections.Generic;

namespace EmailVerifyService.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string? UserName { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string PasswordSalst { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    public virtual ICollection<SessionLog> SessionLogs { get; set; } = new List<SessionLog>();
}

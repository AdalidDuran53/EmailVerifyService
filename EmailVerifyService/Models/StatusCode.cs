using System;
using System.Collections.Generic;

namespace EmailVerifyService.Models;

public partial class StatusCode
{
    public int Id { get; set; }

    public string StatusDescription { get; set; } = null!;

    public virtual ICollection<VerifyCode> VerifyCodes { get; set; } = new List<VerifyCode>();
}

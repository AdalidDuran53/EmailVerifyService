using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class VerificationModel
    {
        public VerificationModel()
        {

        }

        public VerificationModel(string Code, int ExpirationMinutes)
        {

        }
        public string Code { get; set; }
        public int ExpirationMinutes { get; set; }
    }

}

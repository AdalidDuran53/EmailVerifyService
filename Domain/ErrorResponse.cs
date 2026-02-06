using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
    }

}

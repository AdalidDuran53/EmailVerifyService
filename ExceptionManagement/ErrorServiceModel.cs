namespace ExceptionManagement
{
    public class ErroritemServiceModel
    {
        public ErroritemServiceModel()
        {
        }
        public ErroritemServiceModel(string code, string message, string details = "")
        {
            Code = code;
            Message = message;
            Details = details;
        }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
    public class ErrorServiceModel
    {
        public ErrorServiceModel()
        {
            InitializeErrors();
        }

        private Dictionary<string, ErroritemServiceModel> errors = new Dictionary<string, ErroritemServiceModel>();

        public ErroritemServiceModel GetError(string code)
        {
            if (errors.ContainsKey(code))
            {
                return errors[code];
            }
            return new ErroritemServiceModel("UnknownError", "An unknown error occurred.");
        }


        private void InitializeErrors()
        {
            // define all error items here
            #region General
            errors.Add("OMS-GENERAL-ERROR", new ErroritemServiceModel(
                code: "OMS-GENERAL-ERROR", 
                message: "unexpected error.", 
                details: "An unexpected error has occurred in the service. Please try again later or contact the administrator if the problem persists."));
            #endregion

        }
    }
}

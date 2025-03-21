namespace Test003.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

        public ApiResponse(bool success, int code, string message)
        {
            Success = success;
            Code = code;
            Message = message;
        }
    }
}

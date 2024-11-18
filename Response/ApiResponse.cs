namespace EcommercePetsFoodBackend.Response
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Error {  get; set; }
        public T Data { get; set; }
        public ApiResponse(int statuscode, string statusmessage, T data = default(T),string error = null)
        {
            StatusCode = statuscode;
            StatusMessage = statusmessage;
            Error = error;
            Data = data;

        }
    }
}

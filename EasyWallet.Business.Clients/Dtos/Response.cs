namespace EasyWallet.Business.Clients.Dtos
{
    public class Response<T>
    {
        public string Message { get; set; }
        public virtual T Data { get; set; }
        public Error Error { get; set; }

        public Response() { }

        public Response(T data)
        {
            Data = data;
        }
    }

    public class NoDataResponse : Response<object>
    {
        public override object Data => null;

        public NoDataResponse() { }

        public NoDataResponse(string message)
        {
            Message = message;
        }
    }

    public class ErrorResponse : NoDataResponse
    {
        public ErrorResponse(Error error)
        {
            Error = error;
        }
    }
}

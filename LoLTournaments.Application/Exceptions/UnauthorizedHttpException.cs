namespace LoLTournaments.Application.Exceptions
{
    public class UnauthorizedHttpException : HttpException
    {
        public UnauthorizedHttpException(string message) : base(message)
        {
            StatusCode = 401;
        }
    }
}

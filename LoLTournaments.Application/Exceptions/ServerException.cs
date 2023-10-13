namespace CCG.Berserk.Application.Exceptions
{
    /**
	 * If error occured because of server
	 */
    public class ServerException : HttpException
    {
        public ServerException(string message) : base(message)
        {
            StatusCode = 500;
        }
    }
}

namespace CCG.Berserk.Application.Exceptions
{
    /**
	 * If error occured because of client
	 */
    public class ClientException : HttpException
    {
        public ClientException(string message) : base(message)
        {
            StatusCode = 400;
        }
    }
}

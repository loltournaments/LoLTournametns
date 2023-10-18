namespace LoLTournaments.Application.Exceptions
{
	public class AppTokenException : HttpException
	{
		public AppTokenException(string message) : base(message)
		{
			StatusCode = 401;
		}
	}
}
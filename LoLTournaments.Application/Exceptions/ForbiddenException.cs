namespace LoLTournaments.Application.Exceptions
{
	public class ForbiddenException : HttpException
	{
		public ForbiddenException(string message) : base(message)
		{
			StatusCode = 403;
		}
	}
}
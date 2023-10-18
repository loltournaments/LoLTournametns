namespace LoLTournaments.Application.Exceptions
{
	public class ConflictException : HttpException
	{
		public ConflictException(string message) : base(message)
		{
			StatusCode = 409;
		}
	}
}
namespace LoLTournaments.Application.Exceptions
{
	public class NotFoundException : HttpException
	{
		public NotFoundException(string message) : base(message)
		{
			StatusCode = 404;
		}
	}
}
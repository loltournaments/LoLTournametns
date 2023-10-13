namespace CCG.Berserk.Application.Exceptions
{
	public class ForbiddenException : HttpException
	{
		public ForbiddenException(string message) : base(message)
		{
			StatusCode = 403;
		}
	}
}
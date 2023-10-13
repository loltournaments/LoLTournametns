namespace CCG.Berserk.Application.Exceptions
{
	public class ValidationException : HttpException
	{
		public ValidationException(string message) : base(message)
		{
			StatusCode = 400;
		}
	}
}
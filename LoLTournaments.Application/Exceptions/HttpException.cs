#nullable enable
namespace LoLTournaments.Application.Exceptions
{
	public class HttpException : Exception
	{
		public int StatusCode { get; set; }

		public HttpException(string message) : base(message)
		{
		}
	}
}

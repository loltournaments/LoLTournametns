using System;

namespace CCG.Berserk.Application.Exceptions
{
	public class AnonymousException : Exception
	{
		public AnonymousException(string message) : base(message)
		{ }
	}
}
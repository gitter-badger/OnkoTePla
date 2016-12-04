using System;

namespace bytePassion.OnkoTePla.Contracts.Domain.Exceptions
{
	public class VersionNotApplicapleException : Exception
	{
		public VersionNotApplicapleException()
		{			
		}

		public VersionNotApplicapleException(string message)
			: base(message)
		{			
		}

		public VersionNotApplicapleException(string format, params object[] args)
			: base(string.Format(format, args))
		{			
		}

		public VersionNotApplicapleException(string message, Exception innerException)
			: base(message, innerException)
		{			
		}

		public VersionNotApplicapleException(string format, Exception innerException, params object[] args)
			: base(string.Format(format, args), innerException)
		{			
		}
	}
}

using System;

namespace bytePassion.OnkoTePla.Client.Core.Exceptions
{
	public class AppointmentRuleIsNotApplicableException : Exception
	{
		public AppointmentRuleIsNotApplicableException()
		{			
		}

		public AppointmentRuleIsNotApplicableException(string message)
			: base(message)
		{			
		}

		public AppointmentRuleIsNotApplicableException(string format, params object[] args)
			: base(string.Format(format, args))
		{			
		}

		public AppointmentRuleIsNotApplicableException(string message, Exception innerException)
			: base(message, innerException)
		{			
		}

		public AppointmentRuleIsNotApplicableException (string format, Exception innerException, params object[] args)
			: base(string.Format(format, args), innerException)
		{			
		}
	}
}

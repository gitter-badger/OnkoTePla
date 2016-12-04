using System;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;

namespace bytePassion.OnkoTePla.Client.DataAndService.SessionInfo
{
	public class ApplicationStateEventArgs : EventArgs
	{
		public ApplicationStateEventArgs(ApplicationState newApplicationState)
		{
			NewApplicationState = newApplicationState;
		}

		public ApplicationState NewApplicationState { get; }
	}
}
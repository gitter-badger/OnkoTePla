using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class ReleaseLockRequestHandler : RequestHandlerBase
	{		
		private readonly Action actionCompleteCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;
		private readonly Guid medicalPracticeId;
		private readonly Date day;

		public ReleaseLockRequestHandler (Action actionCompleteCallback,
										  ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,
										  Guid medicalPracticeId,
										  Date day,
										  Action<string> errorCallback)
			: base(errorCallback)
		{			
			this.actionCompleteCallback = actionCompleteCallback;
			this.connectionInfoVariable = connectionInfoVariable;
			this.medicalPracticeId = medicalPracticeId;
			this.day = day;
		}

		public override void HandleRequest (RequestSocket socket)
		{
			if (connectionInfoVariable.Value.LoggedInUser == null)
				return;

			HandleRequestHelper<ReleaseLockRequest, ReleaseLockResponse>(
				new ReleaseLockRequest(connectionInfoVariable.Value.SessionId,
									   connectionInfoVariable.Value.LoggedInUser.Id,
									   medicalPracticeId, day),
				socket,
				response => actionCompleteCallback()
			);
		}
	}
}
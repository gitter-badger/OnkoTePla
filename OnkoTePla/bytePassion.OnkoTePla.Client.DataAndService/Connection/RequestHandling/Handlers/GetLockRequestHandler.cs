using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetLockRequestHandler : RequestHandlerBase
	{
		private readonly Action<bool> resultCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;
		private readonly Guid medicalPracticeId;
		private readonly Date day;

		public GetLockRequestHandler(Action<bool> resultCallback, 
									 ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable, 
									 Guid medicalPracticeId, 
									 Date day, 
									 Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.resultCallback = resultCallback;
			this.connectionInfoVariable = connectionInfoVariable;
			this.medicalPracticeId = medicalPracticeId;
			this.day = day;
		}

		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetLockRequest, GetLockResponse>(
				new GetLockRequest(connectionInfoVariable.Value.SessionId, 
								   connectionInfoVariable.Value.LoggedInUser.Id, 
								   medicalPracticeId, day), 	
				socket,
				response => resultCallback(response.Successful)
			);
		}
	}
}
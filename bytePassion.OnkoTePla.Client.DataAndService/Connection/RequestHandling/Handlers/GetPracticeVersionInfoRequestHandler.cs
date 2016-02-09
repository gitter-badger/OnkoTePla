using System;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetPracticeVersionInfoRequestHandler : RequestHandlerBase
	{
		private readonly Action<uint> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;
		private readonly Guid practiceId;
		private readonly Date day;

		public GetPracticeVersionInfoRequestHandler(Action<uint> dataReceivedCallback, 
													ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,
													Guid practiceId, Date day, 
													Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
			this.practiceId = practiceId;
			this.day = day;
		}

		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetPracticeVersionInfoRequest, GetPracticeVersionInfoResponse>(
				new GetPracticeVersionInfoRequest(connectionInfoVariable.Value.SessionId, 
												  connectionInfoVariable.Value.LoggedInUser.Id, 
												  practiceId, day),
				socket,
				response => dataReceivedCallback(response.MedicalPracticeVersion) 	
			);
		}
	}
}
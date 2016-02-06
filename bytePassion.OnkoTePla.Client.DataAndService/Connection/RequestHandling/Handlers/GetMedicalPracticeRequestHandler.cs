using System;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetMedicalPracticeRequestHandler : RequestHandlerBase
	{
		private readonly Action<ClientMedicalPracticeData> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;
		private readonly Guid medicalPracticeId;
		private readonly uint medicalPracticeVersion;

		public GetMedicalPracticeRequestHandler(Action<ClientMedicalPracticeData> dataReceivedCallback, 
												ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,
												Guid medicalPracticeId, uint medicalPracticeVersion, 
												Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
			this.medicalPracticeId = medicalPracticeId;
			this.medicalPracticeVersion = medicalPracticeVersion;
		}

		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetMedicalPracticeRequest, GetMedicalPracticeResponse>(
				new GetMedicalPracticeRequest(connectionInfoVariable.Value.SessionId, 
											  connectionInfoVariable.Value.LoggedInUser.Id, 
											  medicalPracticeId, 
											  medicalPracticeVersion),
				socket,
				response => dataReceivedCallback(response.MedicalPractice) 	
			);
		}
	}
}
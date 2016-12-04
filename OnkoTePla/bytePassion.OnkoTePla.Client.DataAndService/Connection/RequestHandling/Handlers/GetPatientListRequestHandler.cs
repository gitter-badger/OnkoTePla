using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Patients;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetPatientListRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<Patient>> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;		

		public GetPatientListRequestHandler(Action<IReadOnlyList<Patient>> dataReceivedCallback,
											ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable,											
											Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;			
		}

		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetPatientListRequest, GetPatientListResponse>(
				new GetPatientListRequest(connectionInfoVariable.Value.SessionId, 
										  connectionInfoVariable.Value.LoggedInUser.Id),
				socket, 
				response => dataReceivedCallback(response.Patients)  	
			);
		}
	}
}
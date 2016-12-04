using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Appointments;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetAppointmentsOfAPatientRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<AppointmentTransferData>> dataReceivedCallback;
		private readonly ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable;
		private readonly Guid patientId;

		public GetAppointmentsOfAPatientRequestHandler(Action<IReadOnlyList<AppointmentTransferData>> dataReceivedCallback, 
													   ISharedStateReadOnly<ConnectionInfo> connectionInfoVariable, 
													   Guid patientId,
													   Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
			this.patientId = patientId;
		}
		
		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetAppointmentsOfAPatientRequest, GetAppointmentsOfAPatientResponse>(
				new GetAppointmentsOfAPatientRequest(patientId, 
													 connectionInfoVariable.Value.SessionId, 
													 connectionInfoVariable.Value.LoggedInUser.Id),
				socket,
				response => dataReceivedCallback(response.AppointmentList) 	
			);
		}
	}
}
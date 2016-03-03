using System;
using System.Collections.Generic;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling.Handlers
{
	internal class GetAppointmentsOfADayRequestHandler : RequestHandlerBase
	{
		private readonly Action<IReadOnlyList<AppointmentTransferData>, AggregateIdentifier, uint> dataReceivedCallback;
		private readonly ISharedState<ConnectionInfo> connectionInfoVariable;
		private readonly Date day;
		private readonly Guid medicalPracticeId;		

		public GetAppointmentsOfADayRequestHandler(Action<IReadOnlyList<AppointmentTransferData>, AggregateIdentifier, uint> dataReceivedCallback, 
												   ISharedState<ConnectionInfo> connectionInfoVariable,
												   Date day, Guid medicalPracticeId, 
												   Action<string> errorCallback) 
			: base(errorCallback)
		{
			this.dataReceivedCallback = dataReceivedCallback;
			this.connectionInfoVariable = connectionInfoVariable;
			this.day = day;
			this.medicalPracticeId = medicalPracticeId;			
		}

		public override void HandleRequest(RequestSocket socket)
		{
			HandleRequestHelper<GetAppointmentsOfADayRequest, GetAppointmentsOfADayResponse>(
				new GetAppointmentsOfADayRequest(day, medicalPracticeId, 
												 connectionInfoVariable.Value.SessionId, 
												 connectionInfoVariable.Value.LoggedInUser.Id),
				socket,
				response => dataReceivedCallback(response.AppointmentList, 
												 new AggregateIdentifier(day, response.MedicalPracticeId, response.MedicalPracticeVersion),
												 response.AggregateVersion) 	
				
			);
		}
	}
}
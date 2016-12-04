using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Domain.EventStreamUtils;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetAppointemntsOfAPatientResponseHandler : ResponseHandlerBase<GetAppointmentsOfAPatientRequest>
	{
		private readonly IDataCenter dataCenter;				

		public GetAppointemntsOfAPatientResponseHandler (ICurrentSessionsInfo sessionRepository, 
												         ResponseSocket socket,
												         IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;			
		}

		public override void Handle(GetAppointmentsOfAPatientRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId))
				return;

			var eventStream = dataCenter.GetEventStreamForAPatient(request.PatientId);
			var eventStreamAggregator = new EventStreamAggregator<Guid>(eventStream);
			
			Socket.SendNetworkMsg(new GetAppointmentsOfAPatientResponse(
									(IReadOnlyList<AppointmentTransferData>) eventStreamAggregator.AppointmentData
								 )
			);
		}
	}
}
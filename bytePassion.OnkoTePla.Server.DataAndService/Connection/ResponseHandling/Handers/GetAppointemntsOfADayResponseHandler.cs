using System.Linq;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Core.Domain;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetAppointemntsOfADayResponseHandler : ResponseHandlerBase<GetAppointmentsOfADayRequest>
	{
		private readonly IReadModelRepository readModelRepository;

		public GetAppointemntsOfADayResponseHandler(ICurrentSessionsInfo sessionRepository, 
												    ResponseSocket socket,
												    IReadModelRepository readModelRepository) 
			: base(sessionRepository, socket)
		{
			this.readModelRepository = readModelRepository;
		}

		public override void Handle(GetAppointmentsOfADayRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId, request.MedicalPracticeId))
				return;

			var appointmentSetOfADay = readModelRepository.GetAppointmentSetOfADay(new AggregateIdentifier(request.Day,
																										   request.MedicalPracticeId),
																				   null);		
			Socket.SendNetworkMsg(
				new GetAppointmentsOfADayResponse(request.MedicalPracticeId,
					appointmentSetOfADay.MedicalPracticeVersion,
					appointmentSetOfADay.AggregateVersion,
					appointmentSetOfADay.Appointments
										.Select(appointment => new AppointmentTransferData(appointment))
										.ToList())
			);
		}
	}
}
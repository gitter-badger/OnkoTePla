using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Domain;
using bytePassion.OnkoTePla.Server.DataAndService.EventStreamUtils;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.EventStore;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetAppointemntsOfADayResponseHandler : ResponseHandlerBase<GetAppointmentsOfADayRequest>
	{
		private readonly IEventStore eventStore;		

		public GetAppointemntsOfADayResponseHandler(ICurrentSessionsInfo sessionRepository, 
												    ResponseSocket socket,
												    IEventStore eventStore) 
			: base(sessionRepository, socket)
		{
			this.eventStore = eventStore;			
		}

		public override void Handle(GetAppointmentsOfADayRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId, request.MedicalPracticeId))
				return;

			var eventStream = eventStore.GetEventStreamForADay(new AggregateIdentifier(request.Day, request.MedicalPracticeId));
			var eventStreamAggregator = new EventStreamAggregator(eventStream, request.AggregateVersionLimit);
			
			Socket.SendNetworkMsg(
				new GetAppointmentsOfADayResponse(eventStream.Id.MedicalPracticeId,
												  eventStream.Id.PracticeVersion,
												  eventStreamAggregator.AggregateVersion,
												  eventStreamAggregator.AppointmentData)
			);
		}
	}
}
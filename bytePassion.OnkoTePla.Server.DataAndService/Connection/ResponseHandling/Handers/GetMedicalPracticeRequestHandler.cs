using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetMedicalPracticeRequestHandler : ResponseHandlerBase<GetMedicalPracticeRequest>
	{
		private readonly IDataCenter dataCenter;

		public GetMedicalPracticeRequestHandler(ICurrentSessionsInfo sessionRepository, 
												ResponseSocket socket,
												IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(GetMedicalPracticeRequest request)
		{
			if (!ValidateRequest(request.SessionId, request.UserId, request.MedicalPracticeId))
				return;

			var medicalPractice = dataCenter.GetMedicalPractice(request.MedicalPracticeId, request.MedicalPraciceVersion);
			var practiceData = new ClientMedicalPracticeData(medicalPractice);

			Socket.SendNetworkMsg(new GetMedicalPracticeResponse(practiceData));
		}
	}
}
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetPracticeVersionInfoResponseHandler : ResponseHandlerBase<GetPracticeVersionInfoRequest>
	{
		private readonly IDataCenter dataCenter;

		public GetPracticeVersionInfoResponseHandler (ICurrentSessionsInfo sessionRepository, 
												      ResponseSocket socket,
												      IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(GetPracticeVersionInfoRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId, request.MedicalPracticeId))
				return;

			var practiceVersion = dataCenter.GetMedicalPracticeVersion(request.MedicalPracticeId,
																	   request.Day);

			Socket.SendNetworkMsg(new GetPracticeVersionInfoResponse(practiceVersion));
		}
	}
}
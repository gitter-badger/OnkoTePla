using System.Linq;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetPatientListResponseHandler : ResponseHandlerBase<GetPatientListRequest>
	{
		private readonly IDataCenter dataCenter;

		public GetPatientListResponseHandler(ICurrentSessionsInfo sessionRepository, 
											 ResponseSocket socket,
											 IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(GetPatientListRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId))
				return;

			var patientsToDeliver = request.LoadOnlyAlivePatients
										? dataCenter.GetAllPatients().Where(patient => patient.Alive).ToList()
										: dataCenter.GetAllPatients().ToList();

			Socket.SendNetworkMsg(new GetPatientListResponse(patientsToDeliver));
		}
	}
}
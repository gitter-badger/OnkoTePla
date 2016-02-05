using System.Linq;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetTherapyPlaceTypeListResponseHandler : ResponseHandlerBase<GetTherapyPlacesTypeListRequest>
	{
		private readonly IDataCenter dataCenter;

		public GetTherapyPlaceTypeListResponseHandler(ICurrentSessionsInfo sessionRepository, 
													  ResponseSocket socket,
													  IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(GetTherapyPlacesTypeListRequest request)
		{
			if (!IsRequestValid(request.SessionId, request.UserId))
				return;

			Socket.SendNetworkMsg(new GetTherapyPlacesTypeListResponse(dataCenter.GetAllTherapyPlaceTypes().ToList()));
		}
	}
}
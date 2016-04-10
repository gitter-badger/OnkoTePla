using System.Linq;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetLabelListResponseHandler : ResponseHandlerBase<GetLabelListRequest>
	{
		private readonly IDataCenter dataCenter;

		public GetLabelListResponseHandler (ICurrentSessionsInfo sessionRepository, 
										    ResponseSocket socket, 
										    IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(GetLabelListRequest request)
		{			
			if (!IsRequestValid(request.SessionId))
				return;
		
			var userList = dataCenter.GetAllLabels()
									 .ToList();									
			
			Socket.SendNetworkMsg(new GetLabelListResponse(userList));
		}
	}
}
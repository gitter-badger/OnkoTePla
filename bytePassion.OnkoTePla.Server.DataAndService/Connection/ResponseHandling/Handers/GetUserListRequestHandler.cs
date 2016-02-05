using System.Linq;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetUserListRequestHandler : ResponseHandlerBase<GetUserListRequest>
	{
		private readonly IDataCenter dataCenter;

		public GetUserListRequestHandler(ICurrentSessionsInfo sessionRepository, 
										 ResponseSocket socket, 
										 IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(GetUserListRequest request)
		{			
			if (!ValidateRequest(request.SessionId))
				return;

			var userList = dataCenter.GetAllUsers()
				.Where(user => !user.IsHidden)
				.Select(user => new ClientUserData(user.ToString(), user.Id))
				.ToList();

			Socket.SendNetworkMsg(new GetUserListResponse(userList));
		}
	}
}
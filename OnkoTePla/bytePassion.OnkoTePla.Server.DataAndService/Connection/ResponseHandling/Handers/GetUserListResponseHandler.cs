using System.Linq;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers
{
	internal class GetUserListResponseHandler : ResponseHandlerBase<GetUserListRequest>
	{
		private readonly IDataCenter dataCenter;

		public GetUserListResponseHandler(ICurrentSessionsInfo sessionRepository, 
										  ResponseSocket socket, 
										  IDataCenter dataCenter) 
			: base(sessionRepository, socket)
		{
			this.dataCenter = dataCenter;
		}

		public override void Handle(GetUserListRequest request)
		{			
			if (!IsRequestValid(request.SessionId))
				return;

			var userList = dataCenter.GetAllUsers()
									 .Where(user => !user.IsHidden)
									 .Where(user => user.ListOfAccessableMedicalPractices.Count > 0)
									 .Select(user => new ClientUserData(user.ToString(), user.Id, user.ListOfAccessableMedicalPractices))
									 .ToList();
			
			Socket.SendNetworkMsg(new GetUserListResponse(userList));
		}
	}
}
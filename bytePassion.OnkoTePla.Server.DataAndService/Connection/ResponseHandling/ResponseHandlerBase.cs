using System;
using System.Linq;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling
{
	internal abstract class ResponseHandlerBase<TRequest> : IResponseHandler<TRequest> 
		where TRequest : NetworkMessageBase
	{
		protected ResponseHandlerBase(ICurrentSessionsInfo sessionRepository, 
									  ResponseSocket socket)
		{			
			SessionRepository = sessionRepository;
			Socket = socket;
		}

		public abstract void Handle(TRequest request);
		
		protected ICurrentSessionsInfo SessionRepository { get; }
		protected ResponseSocket       Socket            { get; }

		
		protected bool IsRequestValid (ConnectionSessionId sessionId,
									   Guid? userId = null,
									   Guid? accessedMedPracticeId = null)
		{
			if (!SessionRepository.DoesSessionExist(sessionId))
			{
				const string errorMsg = "the session-ID is invalid";
				Socket.SendNetworkMsg(new ErrorResponse(errorMsg));
				return false;
			}

			if (userId.HasValue)
			{
				var sessionInfo = SessionRepository.GetSessionInfo(sessionId);

				if (sessionInfo.LoggedInUser.Id != userId.Value)
				{
					const string errorMsg = "the user is not logged in";
					Socket.SendNetworkMsg(new ErrorResponse(errorMsg));
					return false;
				}

				if (accessedMedPracticeId.HasValue)
					if (!sessionInfo.LoggedInUser.ListOfAccessableMedicalPractices.Contains(accessedMedPracticeId.Value))
					{
						const string errorMsg = "the user has not the right to access this medical Practice";
						Socket.SendNetworkMsg(new ErrorResponse(errorMsg));
						return false;
					}
			}
			return true;
		}
	}
}

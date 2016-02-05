using System;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling
{
	internal class ResponseHandlerFactory : IResponseHandlerFactory
	{
		private readonly IDataCenter dataCenter;
		private readonly IReadModelRepository readModelRepository;
		private readonly ICurrentSessionsInfo sessionRespository;
		private readonly IHeartbeatThreadCollection heartbeatThreadCollection;

		public ResponseHandlerFactory(IDataCenter dataCenter, 
									  IReadModelRepository readModelRepository, 
									  ICurrentSessionsInfo sessionRespository,
									  IHeartbeatThreadCollection heartbeatThreadCollection)
		{
			this.dataCenter = dataCenter;
			this.readModelRepository = readModelRepository;
			this.sessionRespository = sessionRespository;
			this.heartbeatThreadCollection = heartbeatThreadCollection;
		}

		public IResponseHandler<TRequest> GetHandler<TRequest>(TRequest request, ResponseSocket socket) 
			where TRequest : NetworkMessageBase
		{
			switch (request.Type)
			{
				case NetworkMessageType.GetUserListRequest:              return (IResponseHandler<TRequest>) new GetUserListResponseHandler            (sessionRespository, socket, dataCenter);
				case NetworkMessageType.LoginRequest:                    return (IResponseHandler<TRequest>) new LoginResponseHandler                  (sessionRespository, socket, dataCenter);
				case NetworkMessageType.LogoutRequest:                   return (IResponseHandler<TRequest>) new LogoutResponseHandler                 (sessionRespository, socket);
				case NetworkMessageType.BeginDebugConnectionRequest:     return (IResponseHandler<TRequest>) new BeginDebugConnectionResponseHandler   (sessionRespository, socket);
				case NetworkMessageType.BeginConnectionRequest:          return (IResponseHandler<TRequest>) new BeginConnectionResponseHandler        (sessionRespository, socket, heartbeatThreadCollection);				
				case NetworkMessageType.EndConnectionRequest:            return (IResponseHandler<TRequest>) new EndConnectionResponseHandler          (sessionRespository, socket, heartbeatThreadCollection);
				case NetworkMessageType.GetAccessablePracticesRequest:   return (IResponseHandler<TRequest>) new GetAccessablePracticesResponseHandler (sessionRespository, socket);
				case NetworkMessageType.GetPatientListRequest:           return (IResponseHandler<TRequest>) new GetPatientListResponseHandler         (sessionRespository, socket, dataCenter);
				case NetworkMessageType.GetAppointmentsOfADayRequest:    return (IResponseHandler<TRequest>) new GetAppointemntsOfADayResponseHandler  (sessionRespository, socket, readModelRepository);
				case NetworkMessageType.GetMedicalPracticeRequest:       return (IResponseHandler<TRequest>) new GetMedicalPracticeResponseHandler     (sessionRespository, socket, dataCenter);
				case NetworkMessageType.GetTherapyPlacesTypeListRequest: return (IResponseHandler<TRequest>) new GetTherapyPlaceTypeListResponseHandler(sessionRespository, socket, dataCenter);

				default:
					throw new NotImplementedException();
			}
		}
	}
}

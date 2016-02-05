using System;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling.Handers;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads;
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
		private readonly HeartbeatThreadCollection heartbeatThreadCollection;

		public ResponseHandlerFactory(IDataCenter dataCenter, 
									  IReadModelRepository readModelRepository, 
									  ICurrentSessionsInfo sessionRespository,
									  HeartbeatThreadCollection heartbeatThreadCollection)
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
				case NetworkMessageType.GetUserListRequest:              return (IResponseHandler<TRequest>) new GetUserListRequestHandler            (sessionRespository, socket, dataCenter);
				case NetworkMessageType.LoginRequest:                    return (IResponseHandler<TRequest>) new LoginRequestHandler                  (sessionRespository, socket, dataCenter);
				case NetworkMessageType.LogoutRequest:                   return (IResponseHandler<TRequest>) new LogoutRequestHandler                 (sessionRespository, socket);
				case NetworkMessageType.BeginConnectionRequest:          return (IResponseHandler<TRequest>) new BeginConnectionRequestHandler        (sessionRespository, socket, heartbeatThreadCollection);
				case NetworkMessageType.BeginDebugConnectionRequest:     return (IResponseHandler<TRequest>) new BeginDebugConnectionRequestHandler   (sessionRespository, socket);
				case NetworkMessageType.EndConnectionRequest:            return (IResponseHandler<TRequest>) new EndConnectionRequestHandler          (sessionRespository, socket, heartbeatThreadCollection);
				case NetworkMessageType.GetAccessablePracticesRequest:   return (IResponseHandler<TRequest>) new GetAccessablePracticesRequestHandler (sessionRespository, socket);
				case NetworkMessageType.GetPatientListRequest:           return (IResponseHandler<TRequest>) new GetPatientListRequestHandler         (sessionRespository, socket, dataCenter);
				case NetworkMessageType.GetAppointmentsOfADayRequest:    return (IResponseHandler<TRequest>) new GetAppointemntsOfADayRequestHandler  (sessionRespository, socket, readModelRepository);
				case NetworkMessageType.GetMedicalPracticeRequest:       return (IResponseHandler<TRequest>) new GetMedicalPracticeRequestHandler     (sessionRespository, socket, dataCenter);
				case NetworkMessageType.GetTherapyPlacesTypeListRequest: return (IResponseHandler<TRequest>) new GetTherapyPlaceTypeListRequestHandler(sessionRespository, socket, dataCenter);

				default:
					throw new NotImplementedException();
			}
		}
	}
}

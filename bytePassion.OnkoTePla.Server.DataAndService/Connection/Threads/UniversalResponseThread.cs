using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal class UniversalResponseThread : IThread
	{
		private readonly IDataCenter dataCenter;
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly ICurrentSessionsInfo sessionRepository;

		private readonly Action<AddressIdentifier, ConnectionSessionId> newConnectionCallback;		

		private volatile bool stopRunning;		
		
		public UniversalResponseThread (IDataCenter dataCenter, 
								        NetMQContext context, 
										Address serverAddress,
								        ICurrentSessionsInfo sessionRepository,
										Action<AddressIdentifier,ConnectionSessionId> newConnectionCallback)
		{
			this.dataCenter = dataCenter;
			this.context = context;
			this.serverAddress = serverAddress;
			this.sessionRepository = sessionRepository;
			this.newConnectionCallback = newConnectionCallback;		

			stopRunning = false;
			IsRunning = false;
		}		

		public void Run()
		{
			IsRunning = true;

			using (var socket = context.CreateResponseSocket())
			{				
				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.Request);

				while (!stopRunning)
				{																					
					var request = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(1));

					if (request == null)
						continue;
				 
					switch (request.Type)
					{
						case NetworkMessageType.GetUserListRequest:
						{
							ResponseHandler.HandleUserListRequest((UserListRequest)request, sessionRepository, socket, dataCenter);
							break;
						}
						case NetworkMessageType.LoginRequest:
						{
							ResponseHandler.HandleLoginRequest((LoginRequest)request, sessionRepository, socket, dataCenter);
							break;
						}
						case NetworkMessageType.LogoutRequest:
						{
							ResponseHandler.HandleLogoutRequest((LogoutRequest)request, sessionRepository, socket);
							break;
						}
						case NetworkMessageType.BeginConnectionRequest:
						{
							ResponseHandler.HandleBeginConnectionRequest((BeginConnectionRequest)request, sessionRepository, socket, newConnectionCallback);
							break;
						}
						case NetworkMessageType.BeginDebugConnectionRequest:
						{
							ResponseHandler.HandleBeginDebugConnectionRequest((BeginDebugConnectionRequest)request, sessionRepository, socket);
							break;
						}
						case NetworkMessageType.EndConnectionRequest:
						{
							ResponseHandler.HandleEndConnectionRequest((EndConnectionRequest)request, sessionRepository, socket);
							break;
						}
						case NetworkMessageType.GetAccessablePracticesRequest:
						{
							ResponseHandler.HandleGetAccessablePracticesRequest((GetAccessablePracticesRequest) request, sessionRepository, socket);
							break;
						}
					}					
				}
			}

			IsRunning = false;			
		}

		public void Stop()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }		
	}
}
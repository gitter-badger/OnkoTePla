using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.RequestsAndResponses;
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
		
		private volatile bool stopRunning;
		
		
		public UniversalResponseThread (IDataCenter dataCenter, 
								  NetMQContext context, Address serverAddress,
								  ICurrentSessionsInfo sessionRepository)
		{
			this.dataCenter = dataCenter;
			this.context = context;
			this.serverAddress = serverAddress;
			this.sessionRepository = sessionRepository;


			stopRunning = false;
			IsRunning = false;
		}		

		public void Run()
		{
			IsRunning = true;

			using (var socket = context.CreateResponseSocket())
			{				
				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.DataRequest);

				while (!stopRunning)
				{																					
					var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(1));

					if (inMessage == "")
						continue;

					var request = NetworkMessageCoding.Decode(inMessage);

					switch (request.Type)
					{
						case NetworkMessageType.GetUserListRequest: { ResponseHandler.HandleUserListRequest((UserListRequest)request, sessionRepository, socket, dataCenter); break; }
						case NetworkMessageType.LoginRequest:       { ResponseHandler.HandleLoginRequest   ((LoginRequest)   request, sessionRepository, socket, dataCenter); break; }
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
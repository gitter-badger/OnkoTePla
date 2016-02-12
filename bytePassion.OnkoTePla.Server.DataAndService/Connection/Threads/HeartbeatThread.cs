using System;
using System.Threading;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal class HeartbeatThread : IThread
	{
		public event Action<ConnectionSessionId> ClientVanished;

		private readonly NetMQContext context;		
		private readonly Address clientAddress;
		private readonly ConnectionSessionId sessionId;

		private volatile bool stopRunning;


		public HeartbeatThread (NetMQContext context, 								
								Address clientAddress, 
								ConnectionSessionId sessionId)
		{
			this.context = context;			
			this.clientAddress = clientAddress;
			this.sessionId = sessionId;

			stopRunning = false;
			IsRunning = false;
		}

		public void Run ()
		{
		 	IsRunning = true;

			using (var socket = context.CreateRequestSocket())
			{
				socket.Connect(clientAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.Heartbeat);

				while (!stopRunning)
				{
					Thread.Sleep((int) GlobalConstants.HeartbeatIntverval);

					if (stopRunning)
						break;			
							
					socket.SendNetworkMsg(new HeartbeatRequest(sessionId));

					var response = socket.ReceiveNetworkMsg(TimeSpan.FromMilliseconds(GlobalConstants.ServerWaitTimeForHeartbeatResponse));

					if (response != null)					
					{						
						if (((HeartbeatResponse)response).SessionId == sessionId)												
							continue;						
					}

					if (stopRunning)
						break;

					ClientVanished?.Invoke(sessionId);
					break;
				}
			}

			IsRunning = false;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}
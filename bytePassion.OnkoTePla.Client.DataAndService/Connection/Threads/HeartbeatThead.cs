using System;
using System.Windows;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal class HeartbeatThead : IThread
	{
		public event Action ServerVanished;

		private readonly NetMQContext context;		
		private readonly Address clientAddress;
		private readonly ConnectionSessionId sessionId;

		private volatile bool stopRunning;

		public HeartbeatThead (NetMQContext context,
							   Address clientAddress,
							   ConnectionSessionId sessionId)
		{
			this.context = context;
			this.clientAddress = clientAddress;
			this.sessionId = sessionId;

			IsRunning = true;
			stopRunning = false;
		}

		public void Run ()
		{
			bool onceReturnOldId = false;

			using (var socket = context.CreateResponseSocket())
			{
				socket.Bind(clientAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.Heartbeat);

				while (!stopRunning)
				{
					var request = socket.ReceiveNetworkMsg(TimeSpan.FromMilliseconds(GlobalConstants.ClientWaitTimeForHeartbeat));

					if (request == null)
					{
						Application.Current.Dispatcher.Invoke(() => ServerVanished?.Invoke());
						break;
					}

					if (request.Type == NetworkMessageType.HeartbeatRequest)
					{
						var heartbeatRequest = (HeartbeatRequest) request;

						if (heartbeatRequest.SessionId != sessionId)
						{
							if (onceReturnOldId)
								throw new Exception("inner exception");

							onceReturnOldId = true;							
							socket.SendNetworkMsg(new HeartbeatResponse(heartbeatRequest.SessionId));

							continue;
						}
						
						socket.SendNetworkMsg(new HeartbeatResponse(heartbeatRequest.SessionId));
					}
				}			
			}
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; }
	}
}
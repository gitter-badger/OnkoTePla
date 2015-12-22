using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.Heartbeat;
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
					var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(10));

					if (inMessage == "")
					{						
						ServerVanished?.Invoke();
						break;
					}
					
					var request = Request.Parse(inMessage);

					if (request.SessionId != sessionId)
					{
						if (onceReturnOldId)
							throw new Exception("inner exception");

						onceReturnOldId = true;

						var onceResponse = new Response(request.SessionId);
						socket.SendAString(onceResponse.AsString(), TimeSpan.FromSeconds(2));						

						continue;						
					}

					var response = new Response(sessionId);					
					socket.SendAString(response.AsString(), TimeSpan.FromSeconds(2));					
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
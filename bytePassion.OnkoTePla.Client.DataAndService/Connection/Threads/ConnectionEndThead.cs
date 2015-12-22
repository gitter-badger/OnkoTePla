using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.EndConnection;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal class ConnectionEndThead : IThread
	{
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly ConnectionSessionId sessionId;		
		private readonly Action responseCallback;


		public ConnectionEndThead (NetMQContext context,
								   Address serverAddress,
								   ConnectionSessionId sessionId,
								   Action responseCallback)
		{
			this.context = context;
			this.serverAddress = serverAddress;
			this.sessionId = sessionId;
			this.responseCallback = responseCallback;
			IsRunning = true;
		}

		public void Run ()
		{
			using (var socket = context.CreateRequestSocket())
			{
				socket.Connect(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.EndConnection);

				var outMessage = new Request(sessionId).AsString();
				socket.SendAString(outMessage, TimeSpan.FromSeconds(2));

				socket.ReceiveAString(TimeSpan.FromSeconds(2));

				responseCallback();								
			}
		}

		public void Stop ()
		{
		}

		public bool IsRunning { get; }
	}
}
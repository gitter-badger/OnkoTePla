using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.Connection;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	internal class ConnectionThead : IThread
	{
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly Address clientAddress;
		private readonly Action<ConnectionSessionId> responseCallback;


		public ConnectionThead(NetMQContext context, 
			Address serverAddress,
			Address clientAddress,
			Action<ConnectionSessionId> responseCallback)
		{
			this.context = context;
			this.serverAddress = serverAddress;
			this.clientAddress = clientAddress;
			this.responseCallback = responseCallback;
			IsRunning = true;
		}

		public void Run()
		{
			using (var socket = context.CreateRequestSocket())
			{
				socket.Connect(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.BeginConnection);

				
				var outMessage = new Request(clientAddress.Identifier).AsString();
				socket.SendAString(outMessage, TimeSpan.FromSeconds(2));
					
				var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(2));

				if (inMessage == "")
				{
					responseCallback(null);
				}
				else
				{
					var response = Response.Parse(inMessage);
					responseCallback(response.SessionId);
				}					
			}							
		}

		public void Stop()
		{			
		}

		public bool IsRunning { get; }
	}
}
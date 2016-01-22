using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal class DebugConnectionBeginThead : IThread
	{
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly Address clientAddress;
		private readonly Action<ConnectionSessionId> responseCallback;


		public DebugConnectionBeginThead(NetMQContext context, 
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
				socket.Connect(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.BeginDebugConnection);

								
				socket.SendNetworkMsg(new BeginConnectionRequest(clientAddress.Identifier));
					
				var response = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(3));

				responseCallback(((BeginConnectionResponse) response)?.SessionId);
			}							
		}

		public void Stop()
		{			
		}

		public bool IsRunning { get; }
	}
}
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

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal class AcceptDebugConnectionBeginThread : IThread
	{
		public event Action<AddressIdentifier, ConnectionSessionId> NewDebugConnectionEstablished;
		
		private readonly NetMQContext context;
		private readonly Address serverAddress;

		private volatile bool stopRunning;
		

		public AcceptDebugConnectionBeginThread(NetMQContext context, Address serverAddress)
		{
			this.context = context;
			this.serverAddress = serverAddress;

			stopRunning = false;
			IsRunning = false;
		}		

		public void Run()
		{
			IsRunning = true;

			using (var socket = context.CreateResponseSocket())
			{

				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.BeginDebugConnection);

				while (!stopRunning)
				{																
					var request = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(1));

					if (request == null)
						continue;

					if (request.Type == NetworkMessageType.BeginConnectionRequest)
					{
						var newSessionId = new ConnectionSessionId(Guid.NewGuid());

						Application.Current.Dispatcher.Invoke(
							() => NewDebugConnectionEstablished?.Invoke(((BeginConnectionRequest)request).ClientAddress, newSessionId));
						
						socket.SendNetworkMsg(new BeginConnectionResponse(newSessionId));
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
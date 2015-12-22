using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.Connection;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	internal class SessionConnectionThread : IThread
	{
		public event Action<AddressIdentifier, ConnectionSessionId> NewConnectionEstablished;

		private readonly NetMQContext context;
		private readonly Address serverAddress;

		private volatile bool stopRunning;

		

		public SessionConnectionThread(NetMQContext context, Address serverAddress)
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

				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.BeginConnection);

				while (!stopRunning)
				{																
					var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(1));

					if (inMessage == "")
						continue;

					var request = Request.Parse(inMessage);					
					var newSessionId = new ConnectionSessionId(Guid.NewGuid());

					System.Windows.Application.Current.Dispatcher.Invoke(() =>
					{
						NewConnectionEstablished?.Invoke(request.ClientAddress, newSessionId);
					});					

					var response = new Response(newSessionId);
					socket.SendAString(response.AsString());
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
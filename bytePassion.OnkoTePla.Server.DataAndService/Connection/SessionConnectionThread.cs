using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Resources.ZqmUtils;
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
					var inMessage = socket.ReceiveAString();

					var addressIdentifier = AddressIdentifier.GetIpAddressIdentifierFromString(inMessage);
					var newSessionId = new ConnectionSessionId(Guid.NewGuid());

					NewConnectionEstablished?.Invoke(addressIdentifier, newSessionId);

					var outMessage = $"ok;{newSessionId}";

					socket.SendAString(outMessage);
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
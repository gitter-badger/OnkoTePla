using System;
using System.Text;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
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

				while (!stopRunning)
				{
					Encoding encoding = new UTF8Encoding();

					socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort);

					var inMsg = new Msg();
					socket.Receive(ref inMsg);

					inMsg.InitEmpty();
					socket.Receive(ref inMsg);					
					var str = inMsg.Size > 0
									? encoding.GetString(inMsg.Data, 0, inMsg.Size)
									: string.Empty;
					inMsg.Close();

					var addressIdentifier = AddressIdentifier.GetIpAddressIdentifierFromString(str);
					var newSessionId = new ConnectionSessionId(Guid.NewGuid());

					NewConnectionEstablished?.Invoke(addressIdentifier, newSessionId);

					var message = $"ok;{newSessionId}";

					var outMsg = new Msg();
					outMsg.InitPool(encoding.GetByteCount(message));
					encoding.GetBytes(message, 0, message.Length, outMsg.Data, 0);
					socket.Send(ref outMsg, more:false);
					outMsg.Close();
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
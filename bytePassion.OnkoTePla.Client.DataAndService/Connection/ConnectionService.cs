using System;
using System.Windows;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;
using NLog;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection
{
	internal class ConnectionService : IConnectionService
	{
		private readonly ILogger logger;

		public event Action<ConnectionEvent> ConnectionEventInvoked;


		public ConnectionService(ILogger logger)
		{
			this.logger = logger;
			ConnectionStatus = ConnectionStatus.Disconnected;
		}

		public ConnectionStatus ConnectionStatus { get; private set; }
		public Address          ServerAddress    { get; private set; }
		public Address			ClientAddress    { get; private set; }



        public void TryConnect(Address serverAddress, Address clientAddress)
        {
	        ServerAddress = serverAddress;
	        ClientAddress = clientAddress;

			ConnectionStatus = ConnectionStatus.TryingToConnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryConnect);



			Application.Current.Dispatcher.DelayInvoke(
				() =>
				{
					ConnectionStatus = ConnectionStatus.Connected;
					ConnectionEventInvoked?.Invoke(ConnectionEvent.ConnectionEstablished);
				},
				TimeSpan.FromSeconds(2)
			);
		}

        public void TryDisconnect()
        {
			ConnectionStatus = ConnectionStatus.TryingToDisconnect;
			ConnectionEventInvoked?.Invoke(ConnectionEvent.StartedTryDisconnect);

			Application.Current.Dispatcher.DelayInvoke(
				() =>
				{
					ConnectionStatus = ConnectionStatus.Disconnected;
					ConnectionEventInvoked?.Invoke(ConnectionEvent.Disconnected);					
				},
				TimeSpan.FromSeconds(2)
			);
		}
    }

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
			using (var socket = context.CreateResponseSocket())
			{

				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.BeginConnection);

				
//				var inMessage = socket.ReceiveAString();
//
//				var addressIdentifier = AddressIdentifier.GetIpAddressIdentifierFromString(inMessage);
//				var newSessionId = new ConnectionSessionId(Guid.NewGuid());
//
//
//				var outMessage = $"ok;{newSessionId}";
//
//				socket.SendAString(outMessage);
				
			}
		}

		public void Stop()
		{			
		}

		public bool IsRunning { get; }
	}
}

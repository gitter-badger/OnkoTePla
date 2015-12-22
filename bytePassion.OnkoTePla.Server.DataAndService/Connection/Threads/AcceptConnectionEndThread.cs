using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal class AcceptConnectionEndThread : IThread
	{
		public event Action<ConnectionSessionId> ConnectionEnded;
		
		private readonly NetMQContext context;
		private readonly Address serverAddress;

		private volatile bool stopRunning;


		public AcceptConnectionEndThread (NetMQContext context, Address serverAddress)
		{
			this.context = context;
			this.serverAddress = serverAddress;

			stopRunning = false;
			IsRunning = false;
		}

		public void Run ()
		{
			IsRunning = true;

			using (var socket = context.CreateResponseSocket())
			{

				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.BeginConnection);

//				while (!stopRunning)
//				{
//					var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(1));
//
//					if (inMessage == "")
//						continue;
//
//					var request = Request.Parse(inMessage);
//					var newSessionId = new ConnectionSessionId(Guid.NewGuid());
//
//					System.Windows.Application.Current.Dispatcher.Invoke(() =>
//					{
//						ConnectionEnded?.Invoke(newSessionId);
//					});
//
//					var response = new Response(newSessionId);
//					socket.SendAString(response.AsString(), TimeSpan.FromSeconds(2));
//				}
			}

			IsRunning = false;
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}
}
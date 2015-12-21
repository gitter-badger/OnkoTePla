using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using NetMQ;
using NetMQ.Sockets;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public class ConnectionService : DisposingObject, 
								     IConnectionService
	{
		public event Action<ConnectionSessionId> NewSessionStarted;
		public event Action<ConnectionSessionId> SessionTerminated;

		public void InitiateCommunication(Address serverAddress)
		{
			Console.WriteLine("init");
		}

		public void StopCommunication()
		{
			Console.WriteLine("stop");
		}

		public AddressIdentifier GetAddress(ConnectionSessionId sessionId)
		{
			throw new NotImplementedException();
		}

		public TimeSpan GetSessionStartTime(ConnectionSessionId sessionId)
		{
			throw new NotImplementedException();
		}

		private readonly NetMQContext zmqContext;
		

		internal ConnectionService(NetMQContext zmqContext)
		{			
			this.zmqContext = zmqContext;			
		}

		protected override void CleanUp()
		{
			
		}
	}

	internal class SessionConnectionThread : IThread
	{
		private volatile bool stopRunning;

		private readonly ResponseSocket responseSocket;

		public SessionConnectionThread(ResponseSocket responseSocket)
		{
			this.responseSocket = responseSocket;

			stopRunning = false;
			IsRunning = false;
		}

		public void Run()
		{
			IsRunning = true;

			while (!stopRunning)
			{
				
			}

			IsRunning = false;
			stopRunning = false;
		}

		public void Stop()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; private set; }
	}

}
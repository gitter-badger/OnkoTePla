using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestHandling;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal class UniversalRequestThread : IThread
	{
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly TimeoutBlockingQueue<IRequestHandler> workQueue;
				

		private volatile bool stopRunning;
		 
		public UniversalRequestThread (NetMQContext context, 
								       Address serverAddress,			
								       TimeoutBlockingQueue<IRequestHandler> workQueue)
		{
			this.context = context;
			this.serverAddress = serverAddress;
			this.workQueue = workQueue;			

			IsRunning = true;
			stopRunning = false;
		}

		public void Run()
		{
			using (var socket = context.CreateRequestSocket())
			{
				socket.Connect(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.Request);

				while (!stopRunning)
				{
					var workItem = workQueue.TimeoutTake();					

					if (workItem == null)	// Timeout-case
						continue;

					workItem.HandleRequest(socket);
				}									
			}							
		}

		public void Stop()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; }
	}
}
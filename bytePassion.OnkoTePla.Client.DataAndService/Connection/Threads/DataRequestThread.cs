using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal class DataRequestThread : IThread
	{
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly TimeoutBlockingQueue<RequestObject> workQueue;
		private readonly ConnectionSessionId sessionId;


		private volatile bool stopRunning;
		 
		public DataRequestThread (NetMQContext context, 
								  Address serverAddress,			
								  TimeoutBlockingQueue<RequestObject> workQueue,
								  ConnectionSessionId sessionId)
		{
			this.context = context;
			this.serverAddress = serverAddress;
			this.workQueue = workQueue;
			this.sessionId = sessionId;

			IsRunning = true;
			stopRunning = false;
		}

		public void Run()
		{
			using (var socket = context.CreateRequestSocket())
			{
				socket.Connect(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.DataRequest);

				while (!stopRunning)
				{
					var workItem = workQueue.TimeoutTake();					

					if (workItem == null)
						continue;

					switch (workItem.RequestType)
					{
						case NetworkMessageType.GetUserListRequest: { RequestHandler.HandleUserListRequest((UserListRequestObject)workItem, sessionId, socket); break; }
					}
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
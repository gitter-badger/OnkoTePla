using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal class UniversalRequestThread : IThread
	{
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly TimeoutBlockingQueue<RequestObject> workQueue;

		private ConnectionSessionId currentSessionId;


		private volatile bool stopRunning;
		 
		public UniversalRequestThread (NetMQContext context, 
								       Address serverAddress,			
								       TimeoutBlockingQueue<RequestObject> workQueue)
		{
			this.context = context;
			this.serverAddress = serverAddress;
			this.workQueue = workQueue;

			currentSessionId = null;

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

					switch (workItem.RequestType)
					{
						case NetworkMessageType.GetUserListRequest:
						{
							RequestHandler.HandleUserListRequest((UserListRequestObject)workItem, currentSessionId, socket);
							break;
						}
						case NetworkMessageType.LoginRequest:
						{
							RequestHandler.HandleLoginRequest((LoginRequestObject)workItem, currentSessionId, socket);
							break;
						}
						case NetworkMessageType.BeginConnectionRequest:
						{
							RequestHandler.HandleBeginConnectionRequest((BeginConnectionRequestObject)workItem, out currentSessionId, socket);
							break;
						}
						case NetworkMessageType.BeginDebugConnectionRequest:
						{
							RequestHandler.HandleBeginDebugConnectionRequest((BeginDebugConnectionRequestObject)workItem, out currentSessionId, socket);
							break;
						}
						case NetworkMessageType.EndConnectionRequest:
						{
							RequestHandler.HandleEndConnectionRequest((EndConnectionRequestObject)workItem, currentSessionId, socket);
							break;
						}
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
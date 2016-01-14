using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Client.DataAndService.Connection.RequestObjects;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.DataRequests;
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
						case NetworkMessageType.GetUserListRequest:
						{
							var userListRequest = (UserListRequestObject) workItem;

							var outMessage = NetworkMessageCoding.AsString(new UserListRequest(sessionId));
							socket.SendAString(outMessage, TimeSpan.FromSeconds(2));

							var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(5));
							var response = NetworkMessageCoding.Parse(inMessage);

							switch (response.Type)
							{
								case NetworkMessageType.GetUserListResponse:
								{
									var userListResponse = (UserListResponse) response;
									userListRequest.DataReceivedCallback(userListResponse.AvailableUsers);
									break;
								}
							}

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
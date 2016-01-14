using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.DataRequests;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal class DataRequestThread : IThread
	{
		private readonly IDataCenter dataCenter;
		private readonly NetMQContext context;
		private readonly Address serverAddress;
		private readonly IList<SessionInfo> connectedSessions;
		private readonly IList<Guid> loggedInUsers;

		private volatile bool stopRunning;
		
		
		public DataRequestThread (IDataCenter dataCenter, 
								  NetMQContext context, Address serverAddress,
								  IList<SessionInfo> connectedSessions,
								  IList<Guid> loggedInUsers)
		{
			this.dataCenter = dataCenter;
			this.context = context;
			this.serverAddress = serverAddress;
			this.connectedSessions = connectedSessions;
			this.loggedInUsers = loggedInUsers;

			stopRunning = false;
			IsRunning = false;
		}		

		public void Run()
		{
			IsRunning = true;

			using (var socket = context.CreateResponseSocket())
			{				
				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.DataRequest);

				while (!stopRunning)
				{																					
					var inMessage = socket.ReceiveAString(TimeSpan.FromSeconds(1));

					if (inMessage == "")
						continue;

					var request = NetworkMessageCoding.Parse(inMessage);

					switch (request.Type)
					{
						case NetworkMessageType.GetUserListRequest:
						{
							var inRequest = (UserListRequest)request;

							if (connectedSessions.All(session => session.SessionId != inRequest.SessionId))
							{
								var errorResponse = NetworkMessageCoding.AsString(new ErrorResponse("the session-ID is invalid"));
								socket.SendAString(errorResponse, TimeSpan.FromSeconds(2));
								break;
							}

							var userList = dataCenter.GetAllUsers()
													 .Select(user => new ClientUserData(user.ToString(), user.Id))
													 .ToList();

							var response = NetworkMessageCoding.AsString(new UserListResponse(userList));
							socket.SendAString(response, TimeSpan.FromSeconds(2));
							break;
						}
					}					
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
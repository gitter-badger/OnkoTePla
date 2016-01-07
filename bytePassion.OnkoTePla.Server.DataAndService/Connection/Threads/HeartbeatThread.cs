﻿using System;
using System.Threading;
using System.Windows;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.ZmqUtils;
using bytePassion.OnkoTePla.Contracts.NetworkMessages.Heartbeat;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal class HeartbeatThread : IThread
	{
		public event Action<ConnectionSessionId> ClientVanished;

		private readonly NetMQContext context;		
		private readonly Address clientAddress;
		private readonly ConnectionSessionId sessionId;

		private volatile bool stopRunning;


		public HeartbeatThread (NetMQContext context, 								
								Address clientAddress, 
								ConnectionSessionId sessionId)
		{
			this.context = context;			
			this.clientAddress = clientAddress;
			this.sessionId = sessionId;

			stopRunning = false;
			IsRunning = false;
		}

		public void Run ()
		{
		 	IsRunning = true;

			using (var socket = context.CreateRequestSocket())
			{
				socket.Connect(clientAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.Heartbeat);

				while (!stopRunning)
				{
					Thread.Sleep(GlobalConstants.HeartbeatIntverval);
					
					var outMessage = new Request(sessionId);
					socket.SendAString(outMessage.AsString(), TimeSpan.FromSeconds(2));

					var inMessage = socket.ReceiveAString(TimeSpan.FromMilliseconds(GlobalConstants.ServerWaitTimeForHeartbeatResponse));

					if (inMessage != "")					
					{
						var response = Response.Parse(inMessage);

						if (response.SessionId == sessionId)												
							continue;						
					}
					
					Application.Current.Dispatcher.Invoke(() => ClientVanished?.Invoke(sessionId));
					break;
				}
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
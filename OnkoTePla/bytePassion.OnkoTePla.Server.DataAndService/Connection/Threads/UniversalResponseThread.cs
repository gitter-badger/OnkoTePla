﻿using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Resources;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads
{
	internal class UniversalResponseThread : IThread
	{		
		private readonly NetMQContext context;
		private readonly Address serverAddress;		
		private readonly IResponseHandlerFactory responseHandlerFactory;

		private volatile bool stopRunning;		
		
		public UniversalResponseThread (NetMQContext context, 
										Address serverAddress,								       
										IResponseHandlerFactory responseHandlerFactory)
		{
			
			this.context = context;
			this.serverAddress = serverAddress;			
			this.responseHandlerFactory = responseHandlerFactory;

			stopRunning = false;
			IsRunning = false;
		}		

		public void Run()
		{
			IsRunning = true;

			using (var socket = context.CreateResponseSocket())
			{
				socket.Options.Linger = TimeSpan.Zero;

				socket.Bind(serverAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.Request);

				while (!stopRunning)
				{																					
					var request = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(1));

					if (request == null)
						continue;

					var typedRequest = Converter.ChangeTo(request, request.GetType());		// TODO: ist noch unschön
					
					var responseHandler = responseHandlerFactory.GetHandler(typedRequest, socket);
					responseHandler.Handle(typedRequest);					
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
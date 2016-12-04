//using System;
//using bytePassion.Lib.ConcurrencyLib;
//using bytePassion.Lib.Types.Communication;
//using bytePassion.OnkoTePla.Communication.NetworkMessages;
//using bytePassion.OnkoTePla.Communication.NetworkMessages.Notifications;
//using bytePassion.OnkoTePla.Communication.SendReceive;
//using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
//using bytePassion.OnkoTePla.Resources;
//using NetMQ;
//
//namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus
//{
//	public class ClientEventBusThread : IThread
//	{
//		public event Action<DomainEvent> NewEventAvailable;
//
//		private readonly NetMQContext context;
//		private readonly Address clientAddress;
//
//		private volatile bool stopRunning;
//
//		public ClientEventBusThread(NetMQContext context, Address clientAddress)
//		{
//			this.context = context;
//			this.clientAddress = clientAddress;
//		}
//		 
//		public void Run()
//		{
//			IsRunning = true;
//
//			using (var socket = context.CreatePullSocket())
//			{
//				socket.Bind(clientAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.EventBus);
//
//				while (!stopRunning)
//				{
//					var inMsg = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(1));
//					
//					if (inMsg == null)
//						continue;
//
//					switch (inMsg.Type)
//					{
//						case NetworkMessageType.EventBusNotification:
//						{
//							NewEventAvailable?.Invoke(((EventBusNotification)inMsg).NewEvent);
//							break;
//						}
//					}
//				}
//					
//			}
//
//			IsRunning = false;
//		}
//
//		public void Stop()
//		{
//			stopRunning = true;
//		}
//
//		public bool IsRunning { get; private set; }
//	}
//}
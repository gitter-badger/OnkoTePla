//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using System.Linq;
//using bytePassion.Lib.ConcurrencyLib;
//using bytePassion.Lib.Types.Communication;
//using bytePassion.OnkoTePla.Communication.NetworkMessages.Notifications;
//using bytePassion.OnkoTePla.Communication.SendReceive;
//using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
//using bytePassion.OnkoTePla.Resources;
//using NetMQ;
//using NetMQ.Sockets;
//
//namespace bytePassion.OnkoTePla.Communication.NetworkMessageBus
//{
//	public class ServerEventBusThread : IThread
//	{
//		private readonly NetMQContext context;
//		private readonly ObservableCollection<Address> clientAddresses;
//		private readonly TimeoutBlockingQueue<DomainEvent> eventListToPush;
//		private readonly IDictionary<Address, PushSocket> sockets;
//
//		private volatile bool stopRunning;				
//		
//		public ServerEventBusThread (NetMQContext context, 
//									 ObservableCollection<Address> clientAddresses,
//									 TimeoutBlockingQueue<DomainEvent> eventListToPush)
//		{
//			this.context = context;
//			this.clientAddresses = clientAddresses;
//			this.eventListToPush = eventListToPush;
//
//			sockets = new Dictionary<Address, PushSocket>();
//
//			foreach (var clientAddress in clientAddresses)
//			{
//				var newSocket = context.CreatePushSocket();
//				newSocket.Connect(clientAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.EventBus);
//				sockets.Add(clientAddress, newSocket);
//			}		
//
//			clientAddresses.CollectionChanged += OnClientAddressesChanged;
//		}
//		
//		private void OnClientAddressesChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
//		{
//			lock (sockets)
//			{
//				switch (notifyCollectionChangedEventArgs.Action)
//				{
//					case NotifyCollectionChangedAction.Add:
//					{
//						foreach (var newAddress in notifyCollectionChangedEventArgs.NewItems.Cast<Address>())
//						{
//							var newSocket = context.CreatePushSocket();
//							newSocket.Connect(newAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.EventBus);
//							sockets.Add(newAddress, newSocket);
//						}
//						break;
//					}
//					case NotifyCollectionChangedAction.Remove:
//					{
//						foreach (var oldAddress in notifyCollectionChangedEventArgs.OldItems.Cast<Address>())
//						{
//							var socketRemove = sockets[oldAddress];
//							socketRemove.Dispose();
//
//							sockets.Remove(oldAddress);
//						}
//						break;
//					}
//					default:
//						throw new ArgumentException();
//				}
//			}
//
//		}
//
//		public void Run ()
//		{
//			IsRunning = true;
//
//			
//			while (!stopRunning)
//			{
//				var item = eventListToPush.TimeoutTake();
//
//				if (item == null)
//					continue;
//
//				lock (sockets)
//				{
//					foreach (var socket in sockets.Values)
//					{
//						socket.SendNetworkMsg(new EventBusNotification(item));
//					}
//				}
//			}
//
//			CleanUp();
//			IsRunning = false;
//		}
//
//		private void CleanUp()
//		{
//			lock (sockets)
//			{
//				foreach (var socket in sockets.Values)
//				{
//					socket.Dispose();
//				}
//			}
//
//			clientAddresses.CollectionChanged -= OnClientAddressesChanged;
//		}
//
//		public void Stop ()
//		{
//			stopRunning = true;
//		}
//
//		public bool IsRunning { get; private set; }
//	}
//}
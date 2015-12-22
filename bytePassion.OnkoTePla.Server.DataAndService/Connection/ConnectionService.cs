using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public class ConnectionService : DisposingObject, 
								     IConnectionService
	{
		private class SessionInfo
		{
			public ConnectionSessionId SessionId     { get; set; }
			public Time                CreationTime  { get; set; }
			public AddressIdentifier   ClientAddress { get; set; }
		}

		public event Action<ConnectionSessionId> NewSessionStarted;
		public event Action<ConnectionSessionId> SessionTerminated;
		
		private readonly NetMQContext zmqContext;
		private readonly IList<SessionInfo> currentSessions;

		private AcceptConnectionBeginThread acceptConnectionBeginThread;
		private readonly IDictionary<ConnectionSessionId, HeartbeatThread> heartbeatThreads; 

		internal ConnectionService (NetMQContext zmqContext)
		{
			this.zmqContext = zmqContext;

			currentSessions = new List<SessionInfo>();
			heartbeatThreads = new Dictionary<ConnectionSessionId, HeartbeatThread>();
		}

		public void InitiateCommunication(Address serverAddress)
		{
			ServerAddress = serverAddress;
			acceptConnectionBeginThread = new AcceptConnectionBeginThread(zmqContext, serverAddress);
			acceptConnectionBeginThread.NewConnectionEstablished += OnNewConnectionEstablished;			

			var runnableThread = new Thread(acceptConnectionBeginThread.Run);
			runnableThread.Start();			
		}

		private void OnNewConnectionEstablished(AddressIdentifier clientAddress, ConnectionSessionId id)
		{
			currentSessions.Add(new SessionInfo
			{
				ClientAddress = clientAddress,
				SessionId = id,
				CreationTime = TimeTools.GetCurrentTimeStamp().Item2
			});

			var clientAdd = new Address(new TcpIpProtocol(), clientAddress);
			var heartbeatThread = new HeartbeatThread(zmqContext, ServerAddress, clientAdd, id);

			heartbeatThreads.Add(id, heartbeatThread);
			heartbeatThread.ClientVanished += HeartbeatOnClientVanished;
			var runnableThread = new Thread(heartbeatThread.Run);
			runnableThread.Start();

			NewSessionStarted?.Invoke(id);
		}

		private void HeartbeatOnClientVanished(ConnectionSessionId connectionSessionId)
		{
			var heartbeatThread = heartbeatThreads[connectionSessionId];
			heartbeatThread.ClientVanished -= HeartbeatOnClientVanished;
			heartbeatThreads.Remove(connectionSessionId);

			var session = currentSessions.First(s => s.SessionId == connectionSessionId);
			currentSessions.Remove(session);

			Application.Current.Dispatcher.Invoke(() => SessionTerminated?.Invoke(connectionSessionId));
		}

		private Address ServerAddress { get; set; }

		public void StopCommunication()
		{
			foreach (var heartbeatThread in heartbeatThreads.Values)
			{
				heartbeatThread.Stop();
				heartbeatThread.ClientVanished -= HeartbeatOnClientVanished;
			}
			heartbeatThreads.Clear();

			acceptConnectionBeginThread.Stop();

			ServerAddress = null;
		}

		public AddressIdentifier GetAddress(ConnectionSessionId sessionId)
		{
			var info = currentSessions.First(infoObj => infoObj.SessionId == sessionId);
			return info.ClientAddress;
		}

		public Time GetSessionStartTime(ConnectionSessionId sessionId)
		{
			var info = currentSessions.First(infoObj => infoObj.SessionId == sessionId);
			return info.CreationTime;
		}

	
		protected override void CleanUp()
		{
			
		}
	}
}
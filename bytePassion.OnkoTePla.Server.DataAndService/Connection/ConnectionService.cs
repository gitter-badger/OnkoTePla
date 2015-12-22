using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
		private AcceptConnectionEndThread   acceptConnectionEndThread;
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
			new Thread(acceptConnectionBeginThread.Run).Start();

			acceptConnectionEndThread = new AcceptConnectionEndThread(zmqContext, serverAddress);
			acceptConnectionEndThread.ConnectionEnded += OnConnectionEnded;			
			new Thread(acceptConnectionEndThread.Run).Start();
		}

		private void OnConnectionEnded(ConnectionSessionId connectionSessionId)
		{			
			var session = currentSessions.FirstOrDefault(s => s.SessionId == connectionSessionId);

			if (session != null)                                    
			{                                                       
				currentSessions.Remove(session);               
				SessionTerminated?.Invoke(connectionSessionId); 
			}            
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

			var session = currentSessions.FirstOrDefault(s => s.SessionId == connectionSessionId);

			if (session != null)									//
			{														//	session will be null
				currentSessions.Remove(session);					//  when ended by 
				SessionTerminated?.Invoke(connectionSessionId);		//  connectionEndMessage
			}														//
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

			acceptConnectionBeginThread.NewConnectionEstablished -= OnNewConnectionEstablished;
			acceptConnectionBeginThread.Stop();

			acceptConnectionEndThread.ConnectionEnded -= OnConnectionEnded;
			acceptConnectionEndThread.Stop();

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
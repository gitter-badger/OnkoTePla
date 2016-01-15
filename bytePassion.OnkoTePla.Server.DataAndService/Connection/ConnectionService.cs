using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public class ConnectionService : DisposingObject, 
								     IConnectionService
	{		
		public event Action<ConnectionSessionId> NewSessionStarted;
		public event Action<ConnectionSessionId> SessionTerminated;
		
		private readonly NetMQContext zmqContext;
		private readonly IDataCenter dataCenter;
		private readonly IList<SessionInfo> currentSessions;

		private AcceptConnectionBeginThread      acceptConnectionBeginThread;
		private AcceptDebugConnectionBeginThread acceptDebugConnectionBeginThread;
		private AcceptConnectionEndThread        acceptConnectionEndThread;
		private DataResponseThread                dataResponseThread;

		private readonly IDictionary<ConnectionSessionId, HeartbeatThread> heartbeatThreads; 

		internal ConnectionService (NetMQContext zmqContext, IDataCenter dataCenter)
		{
			this.zmqContext = zmqContext;
			this.dataCenter = dataCenter;

			currentSessions = new List<SessionInfo>();
			heartbeatThreads = new Dictionary<ConnectionSessionId, HeartbeatThread>();
		}

		public void InitiateCommunication(Address serverAddress)
		{
			ServerAddress = serverAddress;

			acceptConnectionBeginThread = new AcceptConnectionBeginThread(zmqContext, serverAddress);
			acceptConnectionBeginThread.NewConnectionEstablished += OnNewConnectionEstablished;
			new Thread(acceptConnectionBeginThread.Run).Start();

			acceptDebugConnectionBeginThread = new AcceptDebugConnectionBeginThread(zmqContext, serverAddress);
			acceptDebugConnectionBeginThread.NewDebugConnectionEstablished += OnNewDebugConnectionEstablished;
			new Thread(acceptDebugConnectionBeginThread.Run).Start();

			acceptConnectionEndThread = new AcceptConnectionEndThread(zmqContext, serverAddress);
			acceptConnectionEndThread.ConnectionEnded += OnConnectionEnded;			
			new Thread(acceptConnectionEndThread.Run).Start();
			
			dataResponseThread = new DataResponseThread(dataCenter, zmqContext, serverAddress, currentSessions, null);
			new Thread(dataResponseThread.Run).Start();
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

		private void OnNewDebugConnectionEstablished (AddressIdentifier clientAddress, ConnectionSessionId id)
		{
			currentSessions.Add(new SessionInfo(id, TimeTools.GetCurrentTimeStamp().Item2, clientAddress, true));			

			NewSessionStarted?.Invoke(id);
		}

		private void OnNewConnectionEstablished(AddressIdentifier clientAddress, ConnectionSessionId id)
		{ 
			currentSessions.Add(new SessionInfo(id, TimeTools.GetCurrentTimeStamp().Item2, clientAddress,false));

			var clientAdd = new Address(new TcpIpProtocol(), clientAddress);
			var heartbeatThread = new HeartbeatThread(zmqContext, clientAdd, id);

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

			acceptDebugConnectionBeginThread.NewDebugConnectionEstablished -= OnNewDebugConnectionEstablished;
			acceptDebugConnectionBeginThread.Stop();

			acceptConnectionEndThread.ConnectionEnded -= OnConnectionEnded;
			acceptConnectionEndThread.Stop();

			dataResponseThread.Stop();

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
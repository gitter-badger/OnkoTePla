using System;
using System.Collections.Generic;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public class ConnectionService : DisposingObject, 
								     IConnectionService
	{		
		public event Action<SessionInfo> NewSessionStarted
		{
			add    { sessionRepository.NewSessionStarted += value; }
			remove { sessionRepository.NewSessionStarted -= value; }
		}
		public event Action<SessionInfo> SessionTerminated
		{
			add    { sessionRepository.SessionTerminated += value; }
			remove { sessionRepository.SessionTerminated -= value; }
		}
		public event Action<SessionInfo> LoggedInUserUpdated
		{
			add    { sessionRepository.LoggedInUserUpdated += value; }
			remove { sessionRepository.LoggedInUserUpdated -= value; }
		}
		 
		public SessionInfo GetSessionInfo (ConnectionSessionId id)
		{
			return sessionRepository.GetSessionInfo(id);
		}

		private readonly NetMQContext zmqContext;
		private readonly IDataCenter dataCenter;
		private readonly ICurrentSessionsInfo sessionRepository;
				
		private UniversalResponseThread          universalResponseThread;

		private readonly IDictionary<ConnectionSessionId, HeartbeatThread> heartbeatThreads; 

		internal ConnectionService (NetMQContext zmqContext, IDataCenter dataCenter, ICurrentSessionsInfo sessionRepository)
		{
			this.zmqContext = zmqContext;
			this.dataCenter = dataCenter;
			this.sessionRepository = sessionRepository;
			
			heartbeatThreads = new Dictionary<ConnectionSessionId, HeartbeatThread>();
		}
		
		public void InitiateCommunication(Address serverAddress)
		{									
			universalResponseThread = new UniversalResponseThread(dataCenter, zmqContext, serverAddress, 
																  sessionRepository, OnNewConnectionEstablished);
			new Thread(universalResponseThread.Run).Start();
		}				

		public void StopCommunication()
		{
			foreach (var heartbeatThread in heartbeatThreads.Values)
			{
				heartbeatThread.Stop();
				heartbeatThread.ClientVanished -= HeartbeatOnClientVanished;
			}
			heartbeatThreads.Clear();			

			universalResponseThread.Stop();			
		}

		
		private void OnNewConnectionEstablished (AddressIdentifier clientAddressIdentifier, ConnectionSessionId id)
		{			
			var clientAddress = new Address(new TcpIpProtocol(), clientAddressIdentifier);
			var heartbeatThread = new HeartbeatThread(zmqContext, clientAddress, id);

			heartbeatThreads.Add(id, heartbeatThread);
			heartbeatThread.ClientVanished += HeartbeatOnClientVanished;
			new Thread(heartbeatThread.Run).Start();			
		}

		private void HeartbeatOnClientVanished (ConnectionSessionId connectionSessionId)
		{
			var heartbeatThread = heartbeatThreads[connectionSessionId];
			heartbeatThread.ClientVanished -= HeartbeatOnClientVanished;
			heartbeatThreads.Remove(connectionSessionId);

			if (sessionRepository.DoesSessionExist(connectionSessionId))
			{                                                               //	the session does not exist if it was
				sessionRepository.RemoveSession(connectionSessionId);       //  ended corretly by connectionEndMessage
			}
		}

		protected override void CleanUp()
		{
			
		}
	}
}
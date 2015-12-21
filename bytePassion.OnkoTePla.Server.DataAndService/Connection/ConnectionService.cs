using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
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

		private SessionConnectionThread acceptConnectionThread;

		internal ConnectionService (NetMQContext zmqContext)
		{
			this.zmqContext = zmqContext;

			currentSessions = new List<SessionInfo>();
		}

		public void InitiateCommunication(Address serverAddress)
		{
			acceptConnectionThread = new SessionConnectionThread(zmqContext, serverAddress);
			acceptConnectionThread.NewConnectionEstablished += OnNewConnectionEstablished;

			var runnableThread = new Thread(acceptConnectionThread.Run);
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
			NewSessionStarted?.Invoke(id);
		}

		public void StopCommunication()
		{
			acceptConnectionThread.Stop();
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
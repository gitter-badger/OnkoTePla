using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	internal class HeartbeatThreadCollection : DisposingObject, IHeartbeatThreadCollection
	{
		private readonly NetMQContext zmqContext;
		private readonly ICurrentSessionsInfo sessionRepository;
		private readonly IDictionary<ConnectionSessionId, HeartbeatThread> heartbeatThreads;

		public HeartbeatThreadCollection(NetMQContext zmqContext, ICurrentSessionsInfo sessionRepository)
		{
			this.zmqContext = zmqContext;
			this.sessionRepository = sessionRepository;
			heartbeatThreads = new ConcurrentDictionary<ConnectionSessionId, HeartbeatThread>();
		}

		public void StopThread(ConnectionSessionId sessionId)
		{
			if (heartbeatThreads.ContainsKey(sessionId))
			{
				var heartbeatThread = heartbeatThreads[sessionId];
				heartbeatThread.ClientVanished -= HeartbeatOnClientVanished;
				heartbeatThread.Stop();
				heartbeatThreads.Remove(sessionId);
			}
		}

		public void AddThread(AddressIdentifier clientAddressIdentifier, ConnectionSessionId id)
		{
			var clientAddress = new Address(new TcpIpProtocol(), clientAddressIdentifier);
			var heartbeatThread = new HeartbeatThread(zmqContext, clientAddress, id);

			heartbeatThreads.Add(id, heartbeatThread);
			heartbeatThread.ClientVanished += HeartbeatOnClientVanished;
			new Thread(heartbeatThread.Run).Start();
		}

		private void HeartbeatOnClientVanished (ConnectionSessionId connectionSessionId)
		{
			if (heartbeatThreads.ContainsKey(connectionSessionId))
			{
				var heartbeatThread = heartbeatThreads[connectionSessionId];
				heartbeatThread.ClientVanished -= HeartbeatOnClientVanished;
				heartbeatThreads.Remove(connectionSessionId);
			}

			if (sessionRepository.DoesSessionExist(connectionSessionId))
			{                                                               //	the session does not exist if it was
				sessionRepository.RemoveSession(connectionSessionId);       //  ended corretly by connectionEndMessage
			}
		}

		protected override void CleanUp()
		{
			foreach (var heartbeatThread in heartbeatThreads.Values)
			{
				heartbeatThread.Stop();
				heartbeatThread.ClientVanished -= HeartbeatOnClientVanished;
			}
			heartbeatThreads.Clear();
		}
	}
}

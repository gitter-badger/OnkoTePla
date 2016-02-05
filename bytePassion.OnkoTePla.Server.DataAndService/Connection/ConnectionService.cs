using System;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling;
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
		private readonly IReadModelRepository readModelRepository;

		private readonly ICurrentSessionsInfo       sessionRepository;		
		private          IHeartbeatThreadCollection heartbeatThreadCollection;

		private UniversalResponseThread   universalResponseThread;
				

		internal ConnectionService (NetMQContext zmqContext, 
									IDataCenter dataCenter, 
								    IReadModelRepository readModelRepository)
		{
			sessionRepository = new CurrentSessionsInfo();

			this.zmqContext = zmqContext;
			this.dataCenter = dataCenter;
			this.readModelRepository = readModelRepository;						
		}
		
		public void InitiateCommunication(Address serverAddress)
		{			
			heartbeatThreadCollection = new HeartbeatThreadCollection(zmqContext, sessionRepository);

			var responseHandlerFactory = new ResponseHandlerFactory(dataCenter, readModelRepository, sessionRepository, heartbeatThreadCollection);
			universalResponseThread = new UniversalResponseThread(zmqContext, serverAddress, responseHandlerFactory);
			new Thread(universalResponseThread.Run).Start();
		}				

		public void StopCommunication()
		{	
			sessionRepository.ClearRepository();

			heartbeatThreadCollection?.Dispose();
			heartbeatThreadCollection = null;

			universalResponseThread?.Stop();
			universalResponseThread = null;
		}				

		protected override void CleanUp()
		{	
			StopCommunication();		
		}
	}
}
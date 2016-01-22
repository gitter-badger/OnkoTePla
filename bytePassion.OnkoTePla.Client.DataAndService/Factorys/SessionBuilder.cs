using System;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using NLog;

namespace bytePassion.OnkoTePla.Client.DataAndService.Factorys
{
	public class SessionBuilder : ISessionBuilder
	{
		private ISession Instance { get; set; }

		private IConnectionService connectionService;
		
		public SessionBuilder()
		{
			Instance = null;
		}

		public ISession Build()
		{
			if (Instance != null)
				throw new InvalidOperationException();			

			var sessionLogger    = LogManager.GetLogger("sessionLogger");
			var connectionLogger = LogManager.GetLogger("connectionLogger");

			
			connectionService = new ConnectionService(connectionLogger);
			var workFlow = new ClientWorkflow();

			Instance = new Session(connectionService, workFlow, sessionLogger);
			return Instance;
		}

		public void DisposeSession()
		{
			if (Instance == null)
				throw new InvalidOperationException();

			connectionService.Dispose();
			
			Instance = null;
		}
	}
}

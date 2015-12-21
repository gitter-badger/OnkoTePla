using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;
using NLog;

namespace bytePassion.OnkoTePla.Client.DataAndService.Factorys
{
	public class SessionBuilder : ISessionBuilder
	{
		public ISession Build()
		{
			var sessionLogger    = LogManager.GetLogger("sessionLogger");
			var connectionLogger = LogManager.GetLogger("connectionLogger");

			var connectionService = new ConnectionService(connectionLogger);
			var workFlow = new ClientWorkflow();
			

			return new Session(connectionService, workFlow, sessionLogger);
		}
	}
}

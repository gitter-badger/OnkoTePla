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
			var connectionService = new ConnectionService();
			var workFlow = new ClientWorkflow();
			var logger = LogManager.GetLogger("sessionLogger");

			return new Session(connectionService, workFlow, logger);
		}
	}
}

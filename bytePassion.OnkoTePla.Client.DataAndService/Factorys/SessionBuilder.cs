using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.DataAndService.Workflow;

namespace bytePassion.OnkoTePla.Client.DataAndService.Factorys
{
	public class SessionBuilder : ISessionBuilder
	{
		public ISession Build()
		{
			var connectionService = new ConnectionService();
			var workFlow = new ClientWorkflow();

			return new Session(connectionService, workFlow);
		}
	}
}

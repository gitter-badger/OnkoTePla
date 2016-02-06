using System;
using bytePassion.OnkoTePla.Client.DataAndService.Readmodels;


namespace bytePassion.OnkoTePla.Client.DataAndService.EventBus
{
	public interface IClientEventBus : IDisposable
	{
		void RegisterReadModel   (ReadModelBase readModel);
		void DeregisterReadModel (ReadModelBase readModel);		
	}
}
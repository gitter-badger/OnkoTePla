using System;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus
{
	public interface IClientEventBus : IDisposable
	{
		void RegisterReadModel   (ReadModelBase readModel);
		void DeregisterReadModel (ReadModelBase readModel);		
	}
}
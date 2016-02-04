using System;
using bytePassion.OnkoTePla.Core.Readmodels;

namespace bytePassion.OnkoTePla.Core.Eventsystem
{
	public interface IClientEventBus : IDisposable
	{
		void RegisterReadModel (ReadModelBase readModel);
		void DeregisterReadModel (ReadModelBase readModel);		
	}
}
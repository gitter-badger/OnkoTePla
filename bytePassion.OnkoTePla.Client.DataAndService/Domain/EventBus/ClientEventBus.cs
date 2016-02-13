using System.Collections.Generic;
using System.Windows;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus
{
	public class ClientEventBus : DisposingObject, IClientEventBus
	{
		private readonly IConnectionService connectionService;
		private readonly IList<ReadModelBase> registeredReadModels;

		public ClientEventBus(IConnectionService connectionService)
		{
			this.connectionService = connectionService;
			connectionService.NewDomainEventAvailable += OnNewDomainEventAvailable;

			registeredReadModels = new List<ReadModelBase>();
		}

		private void OnNewDomainEventAvailable(DomainEvent domainEvent)
		{
			var typedEvent = Converter.ChangeTo(domainEvent, domainEvent.GetType());

			foreach (var readModel in registeredReadModels)
			{
				Application.Current.Dispatcher.Invoke(() =>
				{
					readModel.Process(typedEvent);
				});				
			}
		}

		public void RegisterReadModel (ReadModelBase readModel)
		{
			registeredReadModels.Add(readModel);
		}

		public void DeregisterReadModel(ReadModelBase readModel)
		{
			registeredReadModels.Remove(readModel);
		}		

		protected override void CleanUp ()
		{
			connectionService.NewDomainEventAvailable -= OnNewDomainEventAvailable;
		}		
	}
}
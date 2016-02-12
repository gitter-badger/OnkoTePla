using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.Domain.Readmodels.Base;

namespace bytePassion.OnkoTePla.Client.DataAndService.Domain.EventBus
{
	public class ClientEventBus : DisposingObject, IClientEventBus
	{
		private readonly IList<ReadModelBase> registeredReadModels;

		public ClientEventBus()
		{
			registeredReadModels = new List<ReadModelBase>();
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

		}		
	}
}
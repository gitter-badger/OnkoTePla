using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Core.Eventsystem;
using bytePassion.OnkoTePla.Core.Readmodels;

namespace bytePassion.OnkoTePla.Communication.NetworkMessageBus
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
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.State;


namespace bytePassion.Lib.Communication.ViewModel
{
	public class ViewModelComunication<TMessageBase>
	{
		private readonly IMessageBus<TMessageBase> viewModelMessageBus;
		private readonly IStateEngine viewModelStateEngine;

		public ViewModelComunication(IMessageBus<TMessageBase> viewModelMessageBus, 
									 IStateEngine viewModelStateEngine)
		{
			this.viewModelMessageBus = viewModelMessageBus;
			this.viewModelStateEngine = viewModelStateEngine;
		}

	}
}

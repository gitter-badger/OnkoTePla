using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.State;


namespace bytePassion.Lib.Communication.ViewModel
{
	public class ViewModelCommunication<TMessageBase>
	{
		private readonly IMessageBus<TMessageBase> viewModelMessageBus;
		private readonly IStateEngine viewModelStateEngine;

		public ViewModelCommunication(IMessageBus<TMessageBase> viewModelMessageBus, 
									 IStateEngine viewModelStateEngine)
		{
			this.viewModelMessageBus = viewModelMessageBus;
			this.viewModelStateEngine = viewModelStateEngine;
		}

		public void RegisterGlobalViewModelVariable<TVariableType>(string identifier, TVariableType initialValue = default(TVariableType))
		{
			
		}
	}
}

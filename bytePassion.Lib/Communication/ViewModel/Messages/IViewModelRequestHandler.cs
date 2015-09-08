using bytePassion.Lib.Communication.MessageBus;


namespace bytePassion.Lib.Communication.ViewModel.Messages
{
	public interface IViewModelRequestHandler<in TViewModelMessage, out TResult> : IRequestHandler<TViewModelMessage, TResult>
		where TViewModelMessage : ViewModelRequest
	{
	}
}

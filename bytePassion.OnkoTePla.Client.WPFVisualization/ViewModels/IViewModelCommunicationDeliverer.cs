using bytePassion.Lib.Communication.ViewModel;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{
	public interface IViewModelCommunicationDeliverer
	{
		IViewModelCommunication ViewModelCommunication { get; }
	}
}
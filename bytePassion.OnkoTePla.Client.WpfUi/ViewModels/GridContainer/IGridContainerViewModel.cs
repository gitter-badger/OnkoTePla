using System.Collections.ObjectModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer
{
	internal interface IGridContainerViewModel : IViewModel,
												 IViewModelMessageHandler<AsureDayIsLoaded>
	{
		ObservableCollection<IAppointmentGridViewModel> LoadedAppointmentGrids { get; }
		
		int CurrentDisplayedAppointmentGridIndex { get; }

		Size ReportedGridSize { set; }
	}
}

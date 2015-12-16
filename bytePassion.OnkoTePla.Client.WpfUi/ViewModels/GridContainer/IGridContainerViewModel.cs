using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using System.Collections.ObjectModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer
{
    public interface IGridContainerViewModel : IViewModel,
											   IViewModelCommunicationDeliverer
	{
		ObservableCollection<IAppointmentGridViewModel> LoadedAppointmentGrids { get; }
		
		int CurrentDisplayedAppointmentGridIndex { get; }

		Size ReportedGridSize { set; }
	}
}

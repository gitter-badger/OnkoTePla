using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;
using System.Collections.ObjectModel;
using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer
{
    public class GridContainerViewModelSampleData : IGridContainerViewModel 
	{
		public GridContainerViewModelSampleData()
		{
			LoadedAppointmentGrids = new ObservableCollection<IAppointmentGridViewModel>
			                         {
				                         new AppointmentGridViewModelSampleData()
			                         };
			CurrentDisplayedAppointmentGridIndex = 0;
		}

		public ObservableCollection<IAppointmentGridViewModel> LoadedAppointmentGrids { get; }
		public int CurrentDisplayedAppointmentGridIndex { get; }

		public Size ReportedGridSize { set { } }

		public IViewModelCommunication ViewModelCommunication { get; } = null;
		
	    public void Dispose() {	}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

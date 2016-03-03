using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.Types.SemanticTypes;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.GridContainer
{
	internal class GridContainerViewModelSampleData : IGridContainerViewModel 
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

		public void Process (AsureDayIsLoaded message) { }

		public void Dispose() {	}
        public event PropertyChangedEventHandler PropertyChanged;
		
	}
}

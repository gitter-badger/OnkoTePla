using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer
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

		public Size ReportedGridSize { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

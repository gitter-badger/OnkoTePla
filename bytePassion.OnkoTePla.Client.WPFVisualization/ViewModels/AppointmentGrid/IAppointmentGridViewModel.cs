using System;
using System.Collections.ObjectModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid
{
	public interface IAppointmentGridViewModel : IDisposable
	{						
		ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; } 	
		
		ITimeGridViewModel 	TimeGridViewModel { get; }
	}
}

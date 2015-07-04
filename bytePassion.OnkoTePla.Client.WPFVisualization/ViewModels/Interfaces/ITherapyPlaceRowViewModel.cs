using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
{
	public interface ITherapyPlaceRowViewModel : IViewModelBase
	{
		ObservableCollection<IAppointmentViewModel> Appointments { get; }

		Time TimeSlotStart { get; } 
		Time TimeSlotEnd { get; }
		double TimeSlotWidth { set; }
		string TherapyPlaceName { get; }
		double LengthOfOneHour { get; }
		Color RoomColor { get; }
		Guid TherapyPlaceId { get; }
	}
}
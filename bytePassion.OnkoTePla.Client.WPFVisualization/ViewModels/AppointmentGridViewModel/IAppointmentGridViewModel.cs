using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowViewModel;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel
{
	public interface IAppointmentGridViewModel : IViewModelBase
	{
		ICommand ShowPracticeAndDate { get; }

		ICommand CommitChanges  { get; }
		ICommand DiscardChanges { get; }

		void DeleteAppointment(IAppointmentViewModel appointmentViewModel, Appointment appointment, ITherapyPlaceRowViewModel containerRow);
		
		ObservableCollection<TimeSlotLabel>             TimeSlotLabels   { get; }
		ObservableCollection<TimeSlotLine>              TimeSlotLines    { get; }
		ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRows { get; } 

		double CurrentGridWidth  { set; get; }
		double CurrentGridHeight { set; get; }

		IAppointmentViewModel EditingObject          { get; set; }
		OperatingMode         OperatingMode          { get; }
		Date                  CurrentlyDisplayedDate { get; }
	}
}

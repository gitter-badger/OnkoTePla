using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
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

		IAppointmentViewModel EditingObject { get; set; }
		OperatingMode         OperatingMode { get; }
	}
}

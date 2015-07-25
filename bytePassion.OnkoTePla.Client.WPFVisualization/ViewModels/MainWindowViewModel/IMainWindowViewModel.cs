
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentTestViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentOverViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelectorViewModel;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindowViewModel
{
	internal interface IMainWindowViewModel
	{		
		IPatientSelectorViewModel         PatientSelectorViewModel         { get; }
		IAddAppointmentTestViewModel      AddAppointmentTestViewModel      { get; }
		IAppointmentOverViewModel         AppointmentOverViewModel         { get; }
		IAppointmentGridViewModel         AppointmentGridViewModel         { get; }
		IDateSelectorViewModel            DateSelectorViewModel            { get; }
		IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; }
		IRoomSelectorViewModel            RoomSelectorViewModel            { get; }
	}
}

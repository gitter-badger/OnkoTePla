using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentTestView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentOverView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindow
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

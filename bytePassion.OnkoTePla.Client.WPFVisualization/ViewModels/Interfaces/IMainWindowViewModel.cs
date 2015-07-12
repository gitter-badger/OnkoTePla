
namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces
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

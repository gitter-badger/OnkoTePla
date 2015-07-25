using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentTestViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentOverViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelectorViewModel;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindowViewModel
{
	internal class MainWindowViewModelSampleData : IMainWindowViewModel
	{
		public MainWindowViewModelSampleData ()
		{			
			PatientSelectorViewModel         = new PatientSelectorViewModelSampleData();
			AddAppointmentTestViewModel      = new AddAppointmentTestViewModelSampleData();
			AppointmentOverViewModel         = new AppointmentOverViewModelSampleData();
			AppointmentGridViewModel         = new AppointmentGridViewModelSampleData();
			DateSelectorViewModel            = new DateSelectorViewModelSampleData();
			MedicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModelSampleData();
			RoomSelectorViewModel            = new RoomSelectorViewModelSampleData();
		}
		
		public IPatientSelectorViewModel         PatientSelectorViewModel         { get; private set; }
		public IAddAppointmentTestViewModel      AddAppointmentTestViewModel      { get; private set; }
		public IAppointmentOverViewModel         AppointmentOverViewModel         { get; private set; }
		public IAppointmentGridViewModel         AppointmentGridViewModel         { get; private set; }
		public IDateSelectorViewModel            DateSelectorViewModel            { get; private set; }
		public IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; private set; }
		public IRoomSelectorViewModel            RoomSelectorViewModel            { get; private set; }
	}
}

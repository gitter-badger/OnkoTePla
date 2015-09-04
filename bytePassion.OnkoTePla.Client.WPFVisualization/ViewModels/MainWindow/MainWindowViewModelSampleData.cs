using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentTestView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentOverView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindow
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
			RoomFilterViewModel            = new RoomFilterViewModelSampleData();
		}
		
		public IPatientSelectorViewModel         PatientSelectorViewModel         { get; private set; }
		public IAddAppointmentTestViewModel      AddAppointmentTestViewModel      { get; private set; }
		public IAppointmentOverViewModel         AppointmentOverViewModel         { get; private set; }
		public IAppointmentGridViewModel         AppointmentGridViewModel         { get; private set; }
		public IDateSelectorViewModel            DateSelectorViewModel            { get; private set; }
		public IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get; private set; }
		public IRoomFilterViewModel            RoomFilterViewModel            { get; private set; }
	}
}

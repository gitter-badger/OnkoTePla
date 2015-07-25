using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.Commands;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Core.Repositories.Readmodel;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentOverViewModel
{
	public class AppointmentOverViewModel : IAppointmentOverViewModel
	{			
		private readonly Command loadReadModelCommand;
		private readonly ObservableCollection<Appointment> displayedAppointments;
		private readonly ObservableCollection<MedicalPractice> medicalPractices; 

		private AppointmentsOfADayReadModel readModel;


		public AppointmentOverViewModel(IReadModelRepository readModelRepository, 
										IConfigurationReadRepository configurationReadRepository)
		{		
			displayedAppointments = new ObservableCollection<Appointment>();
			medicalPractices = new ObservableCollection<MedicalPractice>(configurationReadRepository.GetAllMedicalPractices());			

			loadReadModelCommand = new Command(
				() =>
				{
					displayedAppointments.Clear();

					if (readModel != null)
					{
						readModel.AppointmentChanged -= OnAppointmentChanged;
						readModel.Dispose();
					}

					var date = Date.Parse(SelectedDateAsString);
					var identifier = new AggregateIdentifier(date, SelectedMedicalPractice.Id);
					readModel = readModelRepository.GetAppointmentsOfADayReadModel(identifier);

					readModel.AppointmentChanged += OnAppointmentChanged;

					foreach (var appointment in readModel.Appointments)					
						displayedAppointments.Add(appointment);											
				});
		}

		private void OnAppointmentChanged(object sender, AppointmentChangedEventArgs appointmentChangedEventArgs)
		{
			if (appointmentChangedEventArgs.ChangeAction == ChangeAction.Added)
			{
				displayedAppointments.Add(appointmentChangedEventArgs.Appointment);
			}
		}

		public ObservableCollection<Appointment>     Appointments     { get { return displayedAppointments; }}
		public ObservableCollection<MedicalPractice> MedicalPractices { get { return medicalPractices;      }}

		public MedicalPractice SelectedMedicalPractice { set; private get; }
		public string          SelectedDateAsString    { set; private get; }

		public ICommand LoadReadModel { get { return loadReadModelCommand; }}	
	}
}

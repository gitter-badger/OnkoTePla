using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage
{
	public class SearchPageViewModel : ISearchPageViewModel
    {
		private string selectedPatient;

		public SearchPageViewModel(IPatientSelectorViewModel patientSelectorViewModel,
								   IGlobalState<Patient> selectedPatientVariable)
		{
			PatientSelectorViewModel = patientSelectorViewModel;

			DeleteAppointment = new ParameterrizedCommand<Guid>(
				guid =>
				{
					
				}
			);

			ModifyAppointment = new ParameterrizedCommand<Guid>(
				guid =>
				{

				}
			);

			ShowPreviousAppoointments = new Command(() =>
			{
				
			});

			HidePreviousAppoointments = new Command(() =>
			{

			});

			SelectedPatient = "- noch kein Patient ausgewählt -";

			DisplayedAppointments = new ObservableCollection<Appointment>();
		}

		public ICommand DeleteAppointment { get; }
		public ICommand ModifyAppointment { get; }
		public ICommand ShowPreviousAppoointments { get; }
		public ICommand HidePreviousAppoointments { get; }

		public IPatientSelectorViewModel PatientSelectorViewModel { get; }

		public string SelectedPatient
		{
			get { return selectedPatient; }
			private set { PropertyChanged.ChangeAndNotify(this, ref selectedPatient, value); }
		}

		public ObservableCollection<Appointment> DisplayedAppointments { get; }

		public event PropertyChangedEventHandler PropertyChanged;
    }
}
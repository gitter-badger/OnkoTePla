using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector
{
	public class MedicalPracticeSelectorViewModel : IMedicalPracticeSelectorViewModel
	{
		private readonly IConfigurationReadRepository configuration;		
		private readonly IGlobalState<Guid>            displayedPracticeState; 

		private MedicalPractice selectedPractice;
		private bool practiceIsSelectable;

		public MedicalPracticeSelectorViewModel (IDataCenter dataCenter,
												 IViewModelCommunication viewModelCommunication)
		{
			configuration = dataCenter.Configuration;			 

			displayedPracticeState = viewModelCommunication.GetGlobalViewModelVariable<Guid>(
				AppointmentGridDisplayedPracticeVariable
			);			
			displayedPracticeState.StateChanged += OnDisplayedPracticeStateChanged;

			var appointmentModificationVariable = viewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable
			);
			appointmentModificationVariable.StateChanged += OnAppointmentModificationVariableChanged;

			AvailableMedicalPractices = new ObservableCollection<MedicalPractice>(configuration.GetAllMedicalPractices());

			SelectedMedicalPractice = configuration.GetMedicalPracticeById(displayedPracticeState.Value);

			PracticeIsSelectable = true;
		}

		private void OnAppointmentModificationVariableChanged(AppointmentModifications appointmentModifications)
		{
			PracticeIsSelectable = appointmentModifications == null;
		}

		private void OnDisplayedPracticeStateChanged (Guid medicalPracticeId)
		{
			selectedPractice = configuration.GetMedicalPracticeById(medicalPracticeId);
			PropertyChanged.Notify(this, nameof(SelectedMedicalPractice));
		}		 		

		public MedicalPractice SelectedMedicalPractice
		{
			get { return selectedPractice; }
			set
			{
				if (!Equals(selectedPractice, value))
				{					
					displayedPracticeState.Value = value.Id;
				}

				PropertyChanged.ChangeAndNotify(this, ref selectedPractice, value);
			}
		}

		public ObservableCollection<MedicalPractice> AvailableMedicalPractices { get; }

		public bool PracticeIsSelectable
		{
			get { return practiceIsSelectable; }
			private set { PropertyChanged.ChangeAndNotify(this, ref practiceIsSelectable, value); }
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

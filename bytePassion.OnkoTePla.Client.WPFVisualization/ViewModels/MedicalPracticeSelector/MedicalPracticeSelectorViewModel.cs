using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector
{
	public class MedicalPracticeSelectorViewModel : IMedicalPracticeSelectorViewModel
	{
		private readonly IConfigurationReadRepository configuration;		
		private readonly GlobalState<Guid>            displayedPracticeState; 

		private MedicalPractice selectedPractice;

		public MedicalPracticeSelectorViewModel (IDataCenter dataCenter, 
												 ViewModelCommunication<ViewModelMessage> viewModelCommunication)
		{
			configuration = dataCenter.Configuration;			 

			displayedPracticeState = viewModelCommunication.GetGlobalViewModelVariable<Guid>(
				AppointmentGridDisplayedPracticeVariable
			);			

			displayedPracticeState.StateChanged += OnDisplayedPracticeStateChanged;

			AvailableMedicalPractices = new ObservableCollection<MedicalPractice>(configuration.GetAllMedicalPractices());

			SelectedMedicalPractice = configuration.GetMedicalPracticeById(displayedPracticeState.Value);
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

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

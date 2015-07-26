using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector
{
	public class MedicalPracticeSelectorViewModel : IMedicalPracticeSelectorViewModel
	{
		private readonly IConfigurationReadRepository          configuration;
		private readonly ObservableCollection<MedicalPractice> availableMedicalPractices;
		private readonly GlobalState<Tuple<Guid, uint>>        displayedPracticeState; 


		private MedicalPractice selectedPractice;

		public MedicalPracticeSelectorViewModel (IConfigurationReadRepository configuration,
												 GlobalState<Tuple<Guid, uint>> displayedPracticeState)
		{
			this.configuration = configuration;
			this.displayedPracticeState = displayedPracticeState;

			displayedPracticeState.StateChanged += OnDisplayedPracticeStateChanged;

			availableMedicalPractices = new ObservableCollection<MedicalPractice>(configuration.GetAllMedicalPractices());

			SelectedMedicalPractice = configuration.GetMedicalPracticeById(displayedPracticeState.Value.Item1);
		}

		private void OnDisplayedPracticeStateChanged (Tuple<Guid, uint> practiceInfo)
		{
			SelectedMedicalPractice = configuration.GetMedicalPracticeById(practiceInfo.Item1);
		}
		 
		public event PropertyChangedEventHandler PropertyChanged;

		public MedicalPractice SelectedMedicalPractice
		{
			get { return selectedPractice; }
			set
			{
				if (!Equals(selectedPractice, value))
					displayedPracticeState.Value =  new Tuple<Guid, uint>(value.Id, value.Version);

				PropertyChanged.ChangeAndNotify(this, ref selectedPractice, value);
			}
		}

		public ObservableCollection<MedicalPractice> AvailableMedicalPractices
		{
			get { return availableMedicalPractices; }
		}
	}
}

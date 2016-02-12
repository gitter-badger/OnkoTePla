﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.Repositories.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.SessionInfo;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector.Helper;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector
{
	internal class MedicalPracticeSelectorViewModel : ViewModel, 
                                                      IMedicalPracticeSelectorViewModel
	{		
		private readonly ISharedState<Guid>                             selectedMedicalPracticeIdVariable;
	    private readonly ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable; 

		private MedicalPracticeDisplayData selectedPractice;
		private bool practiceIsSelectable;
		
		public MedicalPracticeSelectorViewModel (ISession session,
												 IClientMedicalPracticeRepository medicalPracticeRepository,
                                                 ISharedState<Guid> selectedMedicalPracticeIdVariable, 
                                                 ISharedStateReadOnly<AppointmentModifications> appointmentModificationsVariable,
												 Action<string> errorCallback)
		{			
			this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
		   

			
			selectedMedicalPracticeIdVariable.StateChanged += OnSelectedMedicalPracticeIdVariableChanged;
            appointmentModificationsVariable.StateChanged += OnAppointmentModificationVariableChanged;

			AvailableMedicalPractices = session.LoggedInUser
											   .ListOfAccessablePractices
											   .Select(practiceId => new MedicalPracticeDisplayData(practiceId, practiceId.ToString()))
											   .ToObservableCollection();

			foreach (var medicalPracticeDisplayData in AvailableMedicalPractices)
			{
				medicalPracticeRepository.RequestMedicalPractice(
					practice =>
					{
						medicalPracticeDisplayData.PracticeName = practice.Name;
					},
					medicalPracticeDisplayData.MedicalPracticeId,
					errorCallback						
				);
			}

			SelectedMedicalPractice = AvailableMedicalPractices.First(practice => practice.MedicalPracticeId == selectedMedicalPracticeIdVariable.Value);						

			PracticeIsSelectable = true;
		}

        public ObservableCollection<MedicalPracticeDisplayData> AvailableMedicalPractices { get; }         		

		public MedicalPracticeDisplayData SelectedMedicalPractice
		{
			get { return selectedPractice; }
			set
			{
				if (!Equals(selectedPractice, value))
				{					
					selectedMedicalPracticeIdVariable.Value = value.MedicalPracticeId;
				}

				PropertyChanged.ChangeAndNotify(this, ref selectedPractice, value);
			}
		}
		
		public bool PracticeIsSelectable
		{
			get { return practiceIsSelectable; }
			private set { PropertyChanged.ChangeAndNotify(this, ref practiceIsSelectable, value); }
		}

        private void OnAppointmentModificationVariableChanged(AppointmentModifications appointmentModifications)
        {
            PracticeIsSelectable = appointmentModifications == null;
        }

        private void OnSelectedMedicalPracticeIdVariableChanged(Guid medicalPracticeId)
        {
            selectedPractice = AvailableMedicalPractices.First(practice => practice.MedicalPracticeId == medicalPracticeId);
			PropertyChanged.Notify(this, nameof(SelectedMedicalPractice));
        }

        protected override void CleanUp()
        {
            selectedMedicalPracticeIdVariable.StateChanged -= OnSelectedMedicalPracticeIdVariableChanged;
            appointmentModificationsVariable.StateChanged -= OnAppointmentModificationVariableChanged;
        }      
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}

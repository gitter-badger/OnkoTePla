using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.DataAndService.Model;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MedicalPracticeSelector
{
    internal class MedicalPracticeSelectorViewModel : ViewModel, 
                                                      IMedicalPracticeSelectorViewModel
	{
		private readonly IDataCenter                                    dataCenter;		
		private readonly IGlobalState<Guid>                             selectedMedicalPracticeIdVariable;
	    private readonly IGlobalStateReadOnly<AppointmentModifications> appointmentModificationsVariable; 

		private MedicalPractice selectedPractice;
		private bool practiceIsSelectable;

		public MedicalPracticeSelectorViewModel (IDataCenter dataCenter, 
                                                 IGlobalState<Guid> selectedMedicalPracticeIdVariable, 
                                                 IGlobalStateReadOnly<AppointmentModifications> appointmentModificationsVariable)
		{
		    this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;
		    this.appointmentModificationsVariable = appointmentModificationsVariable;
		    this.dataCenter = dataCenter;

			
			selectedMedicalPracticeIdVariable.StateChanged += OnSelectedMedicalPracticeIdVariableChanged;
            appointmentModificationsVariable.StateChanged += OnAppointmentModificationVariableChanged;

			AvailableMedicalPractices = dataCenter.GetAllMedicalPractices()
                                                  .ToObservableCollection();

			SelectedMedicalPractice = dataCenter.GetMedicalPracticeById(selectedMedicalPracticeIdVariable.Value);

			PracticeIsSelectable = true;
		}

        public ObservableCollection<MedicalPractice> AvailableMedicalPractices { get; }         		

		public MedicalPractice SelectedMedicalPractice
		{
			get { return selectedPractice; }
			set
			{
				if (!Equals(selectedPractice, value))
				{					
					selectedMedicalPracticeIdVariable.Value = value.Id;
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
            selectedPractice = dataCenter.GetMedicalPracticeById(medicalPracticeId);
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

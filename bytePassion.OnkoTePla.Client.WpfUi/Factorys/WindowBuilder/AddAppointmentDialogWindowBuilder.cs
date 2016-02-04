using System;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.Data;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WpfUi.Views;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
	internal class AddAppointmentDialogWindowBuilder : IWindowBuilder<AddAppointmentDialog>
	{
		private readonly IDataCenter dataCenter;		                
	    private readonly ISharedStateReadOnly<Guid> selectedMedicalPractiveVariable;       
        private readonly ISharedStateReadOnly<Date> selectedDateVariable;	    	   
	    private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;

	    public AddAppointmentDialogWindowBuilder(IDataCenter dataCenter,												 
                                                 ISharedStateReadOnly<Guid> selectedMedicalPractiveVariable,                                                 
                                                 ISharedStateReadOnly<Date> selectedDateVariable, 												
												 IAppointmentViewModelBuilder appointmentViewModelBuilder)
		{
			this.dataCenter = dataCenter;			
            this.selectedMedicalPractiveVariable = selectedMedicalPractiveVariable;            
            this.selectedDateVariable = selectedDateVariable;	        
		    this.appointmentViewModelBuilder = appointmentViewModelBuilder;
		}

		public AddAppointmentDialog BuildWindow()
		{

            var selectedPatientVariable = new SharedState<Patient>();
		    						
			IPatientSelectorViewModel patientSelectorViewModel = new PatientSelectorViewModel(dataCenter, selectedPatientVariable);

			return new AddAppointmentDialog
			       {
						Owner = Application.Current.MainWindow,
						DataContext = new AddAppointmentDialogViewModel(dataCenter,
                                                                        patientSelectorViewModel, 																		
                                                                        selectedPatientVariable,
                                                                        selectedDateVariable.Value,                                                                         																		
																		selectedMedicalPractiveVariable.Value,
																		appointmentViewModelBuilder)
			       };
		}

		public void DisposeWindow(AddAppointmentDialog buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

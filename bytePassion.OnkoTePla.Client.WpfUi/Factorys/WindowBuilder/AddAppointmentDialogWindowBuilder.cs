using bytePassion.Lib.Communication.State;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.Model;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WpfUi.Views;
using bytePassion.OnkoTePla.Contracts.Patients;
using System;
using System.Windows;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
    internal class AddAppointmentDialogWindowBuilder : IWindowBuilder<AddAppointmentDialog>
	{
		private readonly IDataCenter dataCenter;		                
	    private readonly IGlobalStateReadOnly<Guid> selectedMedicalPractiveVariable;       
        private readonly IGlobalStateReadOnly<Date> selectedDateVariable;	    	   
	    private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;

	    public AddAppointmentDialogWindowBuilder(IDataCenter dataCenter,												 
                                                 IGlobalStateReadOnly<Guid> selectedMedicalPractiveVariable,                                                 
                                                 IGlobalStateReadOnly<Date> selectedDateVariable, 												
												 IAppointmentViewModelBuilder appointmentViewModelBuilder)
		{
			this.dataCenter = dataCenter;			
            this.selectedMedicalPractiveVariable = selectedMedicalPractiveVariable;            
            this.selectedDateVariable = selectedDateVariable;	        
		    this.appointmentViewModelBuilder = appointmentViewModelBuilder;
		}

		public AddAppointmentDialog BuildWindow()
		{

            var selectedPatientVariable = new GlobalState<Patient>();
		    						
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

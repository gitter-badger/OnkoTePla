using System;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.Views;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Factorys.WindowBuilder
{
    public class AddAppointmentDialogWindowBuilder : IWindowBuilder<AddAppointmentDialog>
	{
		private readonly IDataCenter dataCenter;
		private readonly IViewModelCommunication viewModelCommunication;
                
	    private readonly IGlobalStateReadOnly<Guid> selectedMedicalPractiveVariable;

        private readonly IGlobalState<AppointmentModifications> appointmentModificationVariable;
        private readonly IGlobalState<Date> selectedDateVariable;
	    private readonly IGlobalState<Size> gridSizeVariable;

	    private readonly AdornerControl adornerControl;

	    public AddAppointmentDialogWindowBuilder(IDataCenter dataCenter,
												 IViewModelCommunication viewModelCommunication,                                                  
                                                 IGlobalStateReadOnly<Guid> selectedMedicalPractiveVariable, 
                                                 IGlobalState<AppointmentModifications> appointmentModificationVariable, 
                                                 IGlobalState<Date> selectedDateVariable, 
												 IGlobalState<Size> gridSizeVariable,
												 AdornerControl adornerControl)
		{
			this.dataCenter = dataCenter;
			this.viewModelCommunication = viewModelCommunication;
            this.selectedMedicalPractiveVariable = selectedMedicalPractiveVariable;
            this.appointmentModificationVariable = appointmentModificationVariable;
            this.selectedDateVariable = selectedDateVariable;
	        this.gridSizeVariable = gridSizeVariable;
		    this.adornerControl = adornerControl;
		}

		public AddAppointmentDialog BuildWindow()
		{

            var selectedPatientVariable = new GlobalState<Patient>();
		    						
			IPatientSelectorViewModel patientSelectorViewModel = new PatientSelectorViewModel(dataCenter, selectedPatientVariable);

			return new AddAppointmentDialog
			       {
						Owner = Application.Current.MainWindow,
						DataContext = new AddAppointmentDialogViewModel(patientSelectorViewModel, 																		
																		viewModelCommunication,
                                                                        selectedPatientVariable,
                                                                        appointmentModificationVariable,
                                                                        selectedDateVariable,
																		gridSizeVariable,
                                                                        dataCenter, 																		
																		selectedMedicalPractiveVariable.Value,
																		adornerControl)
			       };
		}

		public void DisposeWindow(AddAppointmentDialog buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

﻿using System;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.DataAndService.MedicalPracticeRepository;
using bytePassion.OnkoTePla.Client.DataAndService.PatientRepository;
using bytePassion.OnkoTePla.Client.DataAndService.ReadModelRepository;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.AppointmentViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.PatientSelector;
using bytePassion.OnkoTePla.Client.WpfUi.Views;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WpfUi.Factorys.WindowBuilder
{
	internal class AddAppointmentDialogWindowBuilder : IWindowBuilder<AddAppointmentDialog>
	{
		private readonly IClientPatientRepository patientRepository;
		private readonly IClientReadModelRepository readModelRepository;
		private readonly IClientMedicalPracticeRepository medicalPracticeRepository;
		private readonly ISharedStateReadOnly<Guid> selectedMedicalPractiveVariable;       
        private readonly ISharedStateReadOnly<Date> selectedDateVariable;	    	   
	    private readonly IAppointmentViewModelBuilder appointmentViewModelBuilder;
		
	    public AddAppointmentDialogWindowBuilder(IClientPatientRepository patientRepository,
												 IClientReadModelRepository readModelRepository,
												 IClientMedicalPracticeRepository medicalPracticeRepository,
												 ISharedStateReadOnly<Guid> selectedMedicalPractiveVariable,                                                 
                                                 ISharedStateReadOnly<Date> selectedDateVariable, 												
												 IAppointmentViewModelBuilder appointmentViewModelBuilder)
		{
		    this.patientRepository = patientRepository;
		    this.readModelRepository = readModelRepository;
		    this.medicalPracticeRepository = medicalPracticeRepository;
		    this.selectedMedicalPractiveVariable = selectedMedicalPractiveVariable;            
            this.selectedDateVariable = selectedDateVariable;	        
		    this.appointmentViewModelBuilder = appointmentViewModelBuilder;
		}

		public AddAppointmentDialog BuildWindow(Action<string> errorCallback)
		{

            var selectedPatientVariable = new SharedState<Patient>();
		    						
			IPatientSelectorViewModel patientSelectorViewModel = new PatientSelectorViewModel(patientRepository, selectedPatientVariable, errorCallback);

			return new AddAppointmentDialog
			       {
						Owner = Application.Current.MainWindow,
						DataContext = new AddAppointmentDialogViewModel(medicalPracticeRepository,
																		readModelRepository,
                                                                        patientSelectorViewModel, 																		
                                                                        selectedPatientVariable,
                                                                        selectedDateVariable.Value,                                                                         																		
																		selectedMedicalPractiveVariable.Value,
																		appointmentViewModelBuilder,
																		errorCallback)
			       };
		}

		public void DisposeWindow(AddAppointmentDialog buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

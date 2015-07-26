﻿using bytePassion.OnkoTePla.Client.WPFVisualization.SessionInfo;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AddAppointmentTestViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentOverViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.PatientSelectorViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelectorViewModel;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MainWindowViewModel
{
	internal class MainWindowViewModel : IMainWindowViewModel
	{
		
		private readonly IPatientSelectorViewModel         patientSelectorViewModel;
		private readonly IAddAppointmentTestViewModel      addAppointmentTestViewModel;
		private readonly IAppointmentOverViewModel         appointmentOverViewModel;
		private readonly IAppointmentGridViewModel         appointmentGridViewModel;
		private readonly IDateSelectorViewModel            dateSelectorViewModel;
		private readonly IMedicalPracticeSelectorViewModel medicalPracticeSelectorViewModel;
		private readonly IRoomSelectorViewModel            roomSelectorViewModel;

		private readonly SessionInformation sessionInformation;
		

		public MainWindowViewModel (IPatientSelectorViewModel patientSelectorViewModel,
									IAddAppointmentTestViewModel addAppointmentTestViewModel, 
									IAppointmentOverViewModel appointmentOverViewModel, 
									IAppointmentGridViewModel appointmentGridViewModel,
									IDateSelectorViewModel dateSelectorViewModel,
									IMedicalPracticeSelectorViewModel medicalPracticeSelectorViewModel,
									IRoomSelectorViewModel roomSelectorViewModel,
									SessionInformation sessionInformation)
		{
		    this.patientSelectorViewModel         = patientSelectorViewModel;
			this.addAppointmentTestViewModel      = addAppointmentTestViewModel;
			this.appointmentOverViewModel         = appointmentOverViewModel;
			this.appointmentGridViewModel         = appointmentGridViewModel;
			this.dateSelectorViewModel            = dateSelectorViewModel;
			this.medicalPracticeSelectorViewModel = medicalPracticeSelectorViewModel;
			this.roomSelectorViewModel            = roomSelectorViewModel;

			this.sessionInformation = sessionInformation;
			
		}

		public IPatientSelectorViewModel         PatientSelectorViewModel         { get { return patientSelectorViewModel;         }}
		public IAddAppointmentTestViewModel      AddAppointmentTestViewModel      { get { return addAppointmentTestViewModel;      }}
		public IAppointmentOverViewModel         AppointmentOverViewModel         { get { return appointmentOverViewModel;         }}
		public IAppointmentGridViewModel         AppointmentGridViewModel         { get { return appointmentGridViewModel;         }}
		public IDateSelectorViewModel            DateSelectorViewModel            { get { return dateSelectorViewModel;            }}
		public IMedicalPracticeSelectorViewModel MedicalPracticeSelectorViewModel { get { return medicalPracticeSelectorViewModel; }}
		public IRoomSelectorViewModel            RoomSelectorViewModel            { get { return roomSelectorViewModel;            }}
	}
}
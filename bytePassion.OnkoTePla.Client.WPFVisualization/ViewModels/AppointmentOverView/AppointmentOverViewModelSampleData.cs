using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Enums;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentOverView
{
	public class AppointmentOverViewModelSampleData : IAppointmentOverViewModel
	{
		public AppointmentOverViewModelSampleData()
		{
			var patient = new Patient("karl heinz", new Date(2, 6, 1965), true, Guid.NewGuid(), "1");
			var therapyPlaceType = new TherapyPlaceType("stuhl", TherapyPlaceIconType.ChairType1, Guid.NewGuid());
			var therapyPlace = new TherapyPlace(Guid.NewGuid(), therapyPlaceType, "BehandlungsPlatz1");


			Appointments = new ObservableCollection<Appointment>()
			{
				new Appointment(patient, "test1", therapyPlace, new Date(20,6,2015), new Time(10, 0), new Time(12,0), Guid.NewGuid()),
				new Appointment(patient, "test2", therapyPlace, new Date(20,6,2015), new Time(13, 0), new Time(14,0), Guid.NewGuid()),
				new Appointment(patient, "test3", therapyPlace, new Date(20,6,2015), new Time(14,30), new Time(16,0), Guid.NewGuid()),
				new Appointment(patient, "test4", therapyPlace, new Date(20,6,2015), new Time(17, 0), new Time(20,0), Guid.NewGuid())
			};
		}

		public ObservableCollection<Appointment>     Appointments     { get; set; }
		public ObservableCollection<MedicalPractice> MedicalPractices { get; set; }		

		public MedicalPractice SelectedMedicalPractice { get; set; }		
		public string          SelectedDateAsString    { get; set; }

		public ICommand LoadReadModel { get; set; }		
	}
}

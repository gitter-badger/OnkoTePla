using System;
using System.Collections.Generic;
using xIT.OnkoTePla.Client.WPFVisualization.ViewModels.Interfaces;
using xIT.OnkoTePla.Contracts.Appointments;
using xIT.OnkoTePla.Contracts.DataObjects;
using xIT.OnkoTePla.Contracts.Enums;


namespace xIT.OnkoTePla.Client.WPFVisualization.SampleData
{
	internal class TestViewViewModelMock : ITestViewViewModel
	{
		public TestViewViewModelMock()
		{
			TherapyPlaceType chair = new TherapyPlaceType("chair", TherapyPlaceIconType.ChairType1);
			TherapyPlaceType bed   = new TherapyPlaceType("bed",   TherapyPlaceIconType.BedType1);

			var listOfTherapyPlaces = new List<TherapyPlace>()
			{
				new TherapyPlace(0, chair),
				new TherapyPlace(1, chair),
				new TherapyPlace(2, chair),
				new TherapyPlace(3, bed),
				new TherapyPlace(4, bed)
			};
			TherapyPlaces = listOfTherapyPlaces;

			var listOfPatients = new List<Patient>()
			{
				new Patient("Marcel Käufer",      DateTime.Parse("1956-07-02"), true,   0),
				new Patient("Luca Dittmar",       DateTime.Parse("1985-05-13"), false,  1),
				new Patient("Thorsten Welter",    DateTime.Parse("1961-02-13"), true,   2),
				new Patient("Sigmund Bambach",    DateTime.Parse("1957-06-28"), true,   3),
				new Patient("Wilfried Weber",     DateTime.Parse("1962-09-30"), true,   4),
				new Patient("Dennis Keil",        DateTime.Parse("1966-02-28"), false,  5)
			};
			Patients = listOfPatients;

			Appointments = new List<Appointment>()
			{
				new Appointment(listOfPatients[0], listOfTherapyPlaces[0], new DateTime(2015, 4, 16, 10, 15, 00), new DateTime(2015, 4, 16, 11, 30, 00)),
				new Appointment(listOfPatients[0], listOfTherapyPlaces[3], new DateTime(2015, 4, 20, 10, 15, 00), new DateTime(2015, 4, 20, 11, 30, 00)),
				new Appointment(listOfPatients[1], listOfTherapyPlaces[1], new DateTime(2015, 4, 17, 11, 00, 00), new DateTime(2015, 4, 17, 12, 30, 00)),
				new Appointment(listOfPatients[2], listOfTherapyPlaces[1], new DateTime(2015, 4, 21,  9, 00, 00), new DateTime(2015, 4, 21, 10, 00, 00)),
				new Appointment(listOfPatients[2], listOfTherapyPlaces[4], new DateTime(2015, 4, 16, 15, 30, 00), new DateTime(2015, 4, 16, 16, 30, 00)),
				new Appointment(listOfPatients[3], listOfTherapyPlaces[2], new DateTime(2015, 4, 16, 10, 15, 00), new DateTime(2015, 4, 16, 11, 30, 00))
			};
		}

		public IReadOnlyList<TherapyPlace> TherapyPlaces { get; private set; }
		public IReadOnlyList<Patient>      Patients      { get; private set; }
		public IReadOnlyList<Appointment>  Appointments  { get; private set; }
	}
}

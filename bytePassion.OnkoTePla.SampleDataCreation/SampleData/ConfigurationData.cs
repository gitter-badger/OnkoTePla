using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Enums;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.SampleData
{
    public static class ConfigurationData
	{

		public static void TestLoad()
		{
			IPersistenceService<Configuration> persistenceService = new JsonConfigurationDataStore(GlobalConstants.ConfigJsonPersistenceFile);
			var repo = new ConfigurationRepository(persistenceService);

			repo.LoadRepository();
		}
		

        public static void CreateConfigTestDataAndSaveToJson()
        {
            IPersistenceService<Configuration> persistenceService = new JsonConfigurationDataStore(GlobalConstants.ConfigJsonPersistenceFile);
            var repo = new ConfigurationRepository(persistenceService, GetMedicalPracticeSample());

            repo.PersistRepository();
			MessageBox.Show("fertig");
		}

		private static Configuration GetMedicalPracticeSample()
		{
			var type1 = new TherapyPlaceType("therapieStuhl", TherapyPlaceIconType.ChairType1, Guid.NewGuid());
			var type2 = new TherapyPlaceType("therapieBett",  TherapyPlaceIconType.BedType1,   Guid.NewGuid());

			var therapyPlacesRoom1 = new List<TherapyPlace>();
			var therapyPlacesRoom2 = new List<TherapyPlace>();
			var therapyPlacesRoom3 = new List<TherapyPlace>();

			var therapyPlaceIndex = 1;

			for (var i = 0; i < 20; i++)			
				therapyPlacesRoom1.Add(new TherapyPlace(Guid.NewGuid(), type1.Id, (therapyPlaceIndex++).ToString()));

			for (var i = 0; i < 6; i++)			
				therapyPlacesRoom2.Add(new TherapyPlace(Guid.NewGuid(), type1.Id, (therapyPlaceIndex++).ToString()));

			for (var i = 0; i < 3; i++)			
				therapyPlacesRoom3.Add(new TherapyPlace(Guid.NewGuid(), type2.Id, (therapyPlaceIndex++).ToString()));										

			var rooms = new List<Room>
			{
				new Room(Guid.NewGuid(), "A12", therapyPlacesRoom1, Colors.LightGreen), 
				new Room(Guid.NewGuid(), "A13", therapyPlacesRoom2, Colors.LightPink)				
			};

			var hoursOfOpening = new HoursOfOpening(new Time( 8, 0), new Time( 8, 0), new Time( 8, 0), new Time( 9, 0), new Time(10, 0), new Time( 8, 0), new Time( 8, 0),
													new Time(15, 0), new Time(16, 0), new Time(17, 0), new Time(18, 0), new Time(20, 0), new Time(17, 0), new Time(17, 0),
													true,            true,            true,            true,            true,            false,           false,
													new List<Date> { new Date( 7,7,2015), new Date(8,7,2015) }, 
													new List<Date> { new Date(18,7,2015) });

			var medPrac1 = MedicalPractice.CreateNewMedicalPractice(rooms, "examplePractice1", hoursOfOpening);
			medPrac1 = medPrac1.AddRoom(new Room(Guid.NewGuid(), "A14", therapyPlacesRoom3, Colors.LightBlue));

			var therapyPlacesRoom4 = new List<TherapyPlace>();
			var therapyPlacesRoom5 = new List<TherapyPlace>();

			therapyPlaceIndex = 1;

			for (var i = 0; i < 10; i++)
				therapyPlacesRoom4.Add(new TherapyPlace(Guid.NewGuid(), type1.Id, (therapyPlaceIndex++).ToString()));

			for (var i = 0; i < 10; i++)
				therapyPlacesRoom5.Add(new TherapyPlace(Guid.NewGuid(), type1.Id, (therapyPlaceIndex++).ToString()));
					
			var rooms2 = new List<Room>
			{
				new Room(Guid.NewGuid(), "B2", therapyPlacesRoom4, Colors.LightGreen), 
				new Room(Guid.NewGuid(), "B3", therapyPlacesRoom5, Colors.LightSkyBlue) 				
			};

			var medPrac2 = MedicalPractice.CreateNewMedicalPractice(rooms2, "examplePractice2", hoursOfOpening);

			var user1 = new User("exampleUser1", new List<Guid> { medPrac1.Id              }, "1234", Guid.NewGuid());
			var user2 = new User("exampleUser2", new List<Guid> { medPrac1.Id, medPrac2.Id }, "2345", Guid.NewGuid());

			return new Configuration(
				new List<TherapyPlaceType> { type1,    type2 },
				new List<MedicalPractice> { medPrac1, medPrac2 },
				new List<User> { user1, user2}
			);
		}
	}
}

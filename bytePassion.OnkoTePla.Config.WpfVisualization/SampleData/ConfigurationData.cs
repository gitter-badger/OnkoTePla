using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Enums;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.SampleData
{
	public static class ConfigurationData
	{

		public static void TestLoad()
		{
			IPersistenceService<Configuration> persistenceService = new XmlConfigurationDataStore("config.xml");
			ConfigurationRepository repo = new ConfigurationRepository(persistenceService);

			repo.LoadRepository();
		}

		public static void ConfigToXml()
		{
			IPersistenceService<Configuration> persistenceService = new XmlConfigurationDataStore("config.xml");
			ConfigurationRepository repo = new ConfigurationRepository(persistenceService, GetMedicalPracticeSample());			

			repo.PersistRepository();
		}

		public static Configuration GetMedicalPracticeSample()
		{
			var type1 = new TherapyPlaceType("therapieStuhl", TherapyPlaceIconType.ChairType1, Guid.NewGuid());
			var type2 = new TherapyPlaceType("therapieBett",  TherapyPlaceIconType.BedType1,   Guid.NewGuid());

			var therapyPlacesRoom1 = new List<TherapyPlace>
			{
				new TherapyPlace( 0, type1), new TherapyPlace( 1, type1),
				new TherapyPlace( 2, type1), new TherapyPlace( 3, type1),
				new TherapyPlace( 4, type1), new TherapyPlace( 5, type1),
				new TherapyPlace( 6, type1), new TherapyPlace( 7, type1),
				new TherapyPlace( 8, type1), new TherapyPlace( 9, type1),
				new TherapyPlace(10, type1), new TherapyPlace(11, type1),
				new TherapyPlace(12, type1), new TherapyPlace(13, type1),
				new TherapyPlace(14, type1), new TherapyPlace(15, type1),
				new TherapyPlace(16, type1), new TherapyPlace(17, type1)
			};

			var therapyPlacesRoom2 = new List<TherapyPlace>
			{
				new TherapyPlace(18, type1), new TherapyPlace(19, type1),
				new TherapyPlace(20, type1), new TherapyPlace(21, type1),
				new TherapyPlace(22, type1), new TherapyPlace(23, type1)
			};

			var therapyPlacesRoom3 = new List<TherapyPlace>
			{
				new TherapyPlace(24, type2), 
				new TherapyPlace(25, type2),
				new TherapyPlace(26, type2)
			};		

			var rooms = new List<Room>
			{
				new Room(Guid.NewGuid(), therapyPlacesRoom1), 
				new Room(Guid.NewGuid(), therapyPlacesRoom2)				
			};

			var medPrac1 = MedicalPractice.CreateNeMedicalPractice(rooms, "examplePractice1");
			medPrac1 = medPrac1.AddRoom(new Room(Guid.NewGuid(), therapyPlacesRoom3));

			var therapyPlacesRoom4 = new List<TherapyPlace>
			{
				new TherapyPlace( 0, type1), new TherapyPlace( 1, type1),
				new TherapyPlace( 2, type1), new TherapyPlace( 3, type1),
				new TherapyPlace( 4, type1), new TherapyPlace( 5, type1),
				new TherapyPlace( 6, type1), new TherapyPlace( 7, type1),
				new TherapyPlace( 8, type1), new TherapyPlace( 9, type1)				
			};

			var therapyPlacesRoom5 = new List<TherapyPlace>
			{
				new TherapyPlace(10, type1), new TherapyPlace(11, type1),
				new TherapyPlace(12, type1), new TherapyPlace(13, type1),
				new TherapyPlace(14, type1), new TherapyPlace(15, type1),
				new TherapyPlace(16, type1), new TherapyPlace(17, type1),
				new TherapyPlace(18, type1), new TherapyPlace(19, type1)				
			};

			var rooms2 = new List<Room>
			{
				new Room(Guid.NewGuid(), therapyPlacesRoom4), 
				new Room(Guid.NewGuid(), therapyPlacesRoom5) 				
			};

			var medPrac2 = MedicalPractice.CreateNeMedicalPractice(rooms2, "examplePractice2");

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

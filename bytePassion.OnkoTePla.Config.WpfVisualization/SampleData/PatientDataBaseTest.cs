using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Contracts.Patients;
using Jil;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.SampleData
{

	[DataContract]
	public class Patient2
	{
				
		public Patient2 (string name, Date birthday, bool alive,
					   Guid id, string externalId)
		{
			Name     = name;
			Alive    = alive;
			Birthday = birthday;
			Id       = id;
			ExternalId = externalId;
		}
		
		[DataMember(Name = "Name")]       public string Name       { get; }
		[DataMember(Name = "Alive")]      public bool   Alive      { get; }
		[DataMember(Name = "Birthday")]   public Date   Birthday   { get; }
		[DataMember(Name = "Id")]         public Guid   Id         { get; }
		[DataMember(Name = "ExternalId")] public string ExternalId { get; }


		#region ToString / HashCode / Equals

		public override string ToString ()         => $"{Name} + (* {Birthday})";		
		public override bool   Equals (object obj) => this.Equals(obj, (patient1, patient2) => patient1.Id == patient2.Id);		  
		public override int    GetHashCode ()      => Id.GetHashCode();
	
		#endregion

		public static IComparer<Patient2> GetNameComparer ()
		{
			return new PatientNameSorter();
		}

		private class PatientNameSorter : IComparer<Patient2>
		{
			public int Compare (Patient2 x, Patient2 y)
			{
				return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
			}
		}
	}

	public class JSonPatientDataStoreTest : IPersistenceService<IEnumerable<Patient2>>
	{
		private readonly string filename;

		public JSonPatientDataStoreTest (string filename)
		{
			this.filename = filename;
		}

		public void Persist (IEnumerable<Patient2> data)
		{

			using (var output = new StringWriter())
			{
				JSON.Serialize(data, output, Options.PrettyPrint);
				File.WriteAllText(filename, output.ToString());
			}
		}

		public IEnumerable<Patient2> Load ()
		{
			List<Patient2> patients;
			
			using (var stream = new FileStream(filename, FileMode.Open))
			{
				var settings = new DataContractJsonSerializerSettings();
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<Patient2>), settings);
				patients = (List<Patient2>)serializer.ReadObject(stream);
			}
			return patients;

		}
	}

	public class PatientRepositoryTest
	{
		
		private IDictionary<Guid, Patient2> patients;
		private readonly IPersistenceService<IEnumerable<Patient2>> persistenceService;

		public PatientRepositoryTest (IPersistenceService<IEnumerable<Patient2>> persistenceService)
		{
			this.persistenceService = persistenceService;
			patients = new Dictionary<Guid, Patient2>();
		}
		

		public void AddPatient (string name, Date birthday, bool alive, string externalId)
		{
			var newPatientId = Guid.NewGuid();
			var newPatient = new Patient2(name, birthday, alive, newPatientId, externalId);
			patients.Add(newPatientId, newPatient);			
		}
		
		public void PersistRepository ()
		{
			persistenceService.Persist(patients.Values.ToList());
		}

		public void LoadRepository ()
		{
			patients = persistenceService.Load().ToDictionary(patient => patient.Id,
															  patient => patient);
		}
	}

	public static class PatientDataBaseTest
	{

		public static void TestLoad()
		{
			IPersistenceService<IEnumerable<Patient2>> persistenceService = new JSonPatientDataStoreTest("patientTest.json");
			PatientRepositoryTest repo = new PatientRepositoryTest(persistenceService);			
			repo.LoadRepository();
		}

		
        public static void GenerateJSONPatientsFile()
        {
            IPersistenceService<IEnumerable<Patient2>> persistenceService = new JSonPatientDataStoreTest("patientTest.json");
			PatientRepositoryTest repo = new PatientRepositoryTest(persistenceService);

            var patients = GeneratePatients(10);

            foreach (var patient in patients)
            {
                repo.AddPatient(patient.Name, patient.Birthday, patient.Alive, patient.ExternalId);
            }

            repo.PersistRepository();
        }

        private static readonly Random Rand = new Random();


		private static IEnumerable<Patient> GeneratePatients (int count)
		{
			IReadOnlyList<string> firstNames = FirstNames();
			IReadOnlyList<string> surNames   = SurNames();

			int firstNamesCount = firstNames.Count;
			int surNamesCount = surNames.Count;


			var patients = new List<Patient>(count);			

			for (int i = 0; i < count; i++)
			{
				patients.Add(new Patient(firstNames[Rand.Next(firstNamesCount-1)] + " " + surNames[Rand.Next(surNamesCount-1)],
										 new Date((byte)Rand.Next(1, 28), 
											      (byte)Rand.Next(1, 13), 
												  (ushort)Rand.Next(1936, 1992)), 
										 GetRandomBoolValue(), 
										 Guid.NewGuid(), 
										 i.ToString()));				
			}	
		
			return patients;
		}
			

		private static bool GetRandomBoolValue()
		{	
			return Rand.NextDouble() > 0.2;
		}
				
		private static IReadOnlyList<string> FirstNames()
		{
			return new List<string>
			{
				"Aaron",	 "Adam",	  "Alan",	  "Albert",	 "Alice",	   "Amanda",	"Amy",			"Andrea",	"Andrew",	"Angela",
				"Ann",		 "Anna",	  "Anne",	  "Annie",	 "Anthony",	   "Arthur",	"Barbara",		"Benjamin",	"Beverly",	"Billy",
				"Bobby",	 "Bonnie",	  "Brandon",  "Brenda",	 "Brian",	   "Bruce",		"Carl",			"Carlos",	"Carol",	"Carolyn",
				"Catherine", "Charles",	  "Cheryl",	  "Chris",	 "Christina",  "Christine",	"Christopher",	"Clarence",	"Craig",	"Cynthia",
				"Daniel",	 "David",	  "Deborah",  "Debra",	 "Denise",	   "Dennis",	"Diana",		"Diane",	"Donald",	"Doris",
				"Dorothy",	 "Douglas",	  "Earl",	  "Edward",	 "Elizabeth",  "Emily",		"Eric",			"Ernest",	"Eugene",	"Evelyn",
				"Frances",	 "Frank",	  "Fred",	  "Gary",	 "George",	   "Gerald",	"Gloria",		"Gregory",	"Harold",	"Harry",
				"Henry",	 "Howard",	  "Irene",	  "Jack",	 "Jacqueline", "James",		"Jane",			"Janet",	"Janice",	"Jason",
				"Jean",		 "Jeffrey",	  "Jennifer", "Jeremy",	 "Jerry",	   "Jesse",		"Jessica",		"Jimmy",	"Joan",		"Joe",
				"Jonathan",	 "Joseph",	  "Joshua",	  "Joyce",	 "Juan",	   "Judith",	"Judy",			"Julia",	"Julie",	"Justin",
				"Karen",	 "Katherine", "Kathleen", "Kathryn", "Kathy",	   "Keith",		"Kelly",		"Kenneth",	"Kimberly",	"Larry",
				"Laura",	 "Lawrence",  "Linda",	  "Lisa",	 "Lois",	   "Lori",		"Louis",		"Louise",	"Margaret",	"Maria",
				"Marie",	 "Marilyn",	  "Mark",	  "Martha",	 "Martin",	   "Mary",		"Matthew",		"Melissa",	"Michael",	"Michelle",
				"Mildred",	 "Nancy",	  "Nicholas", "Nicole",	 "Norma",	   "Pamela",	"Patricia",		"Patrick",	"Paul",		"Paula",
				"Peter",	 "Philip",	  "Phillip",  "Phyllis", "Rachel",	   "Ralph",		"Randy",		"Raymond",	"Rebecca",	"Robert",
				"Ronald",	 "Rose",	  "Roy",	  "Ruby",	 "Russell",	   "Ruth",		"Ryan",			"Samuel",	"Sandra",	"Sara",
				"Sarah",	 "Scott",	  "Sean",	  "Sharon",	 "Shawn",	   "Shirley",	"Stephanie",	"Stephen",	"Steve",	"Steven",
				"Susan",	 "Teresa",	  "Terry",	  "Theresa", "Thomas",	   "Timothy",	"Tina",			"Todd",		"Victor",	"Walter",
				"Wanda",	 "Wayne",	  "William",  "Willie"
			};
		}

		private static IReadOnlyList<string> SurNames()
		{
			return new List<string>
			{
				"Adams",		"Alexander",	"Allen",	"Anderson",	"Bailey",		"Baker",		"Barnes",		"Bell",		"Bennett",	"Brooks",
				"Brown",		"Bryant",		"Butler",	"Campbell",	"Carter",		"Clark",		"Coleman",		"Collins",	"Cook",		"Cooper",
				"Cox",			"Davis",		"Diaz",		"Edwards",	"Evans",		"Flores",		"Foster",		"Garcia",	"Gonzales",	"Gonzalez",
				"Gray",			"Green",		"Griffin",	"Hall",		"Harris",		"Henderson",	"Hernandez",	"Hill",		"Howard",	"Hughes",
				"Jackson",		"James",		"Jenkins",	"Johnson",	"Jones",		"Kelly",		"King",			"Lee",		"Lewis",	"Long",
				"Lopez",		"Martin",		"Martinez",	"Miller",	"Mitchell",		"Moore",		"Morgan",		"Morris",	"Murphy",	"Nelson",
				"Parker",		"Patterson",	"Perez",	"Perry",	"Peterson",		"Phillips",		"Powell",		"Price",	"Ramirez",	"Reed",
				"Richardson",	"Rivera",		"Roberts",	"Robinson",	"Rodriguez",	"Rogers",		"Ross",			"Russell",	"Sanchez",	"Sanders",
				"Scott",		"Simmons",		"Smith",	"Stewart",	"Taylor",		"Thomas",		"Thompson",		"Torres",	"Turner",	"Walker",
				"Ward",			"Washington",	"Watson",	"White",	"Williams",		"Wilson",		"Wood",			"Wright",	"Young"
			};
		} 
	}
}

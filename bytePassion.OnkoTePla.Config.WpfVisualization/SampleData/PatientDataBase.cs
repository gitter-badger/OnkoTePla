using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Repositories;
using bytePassion.OnkoTePla.Client.Core.Repositories.Patients;
using bytePassion.OnkoTePla.Client.Resources;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Config.WpfVisualization.SampleData
{
	public static class PatientDataBase
	{

		public static void TestLoad()
		{
			IPersistenceService<IEnumerable<Patient>> persistenceService = new JSonPatientDataStore(GlobalConstants.PatientJsonPersistenceFile);
			var repo = new PatientRepository(persistenceService);

			repo.LoadRepository();
		}


        public static void GenerateJSONPatientsFile(int patientCount)
        {
            IPersistenceService<IEnumerable<Patient>> persistenceService = new JSonPatientDataStore(GlobalConstants.PatientJsonPersistenceFile);
            var repo = new PatientRepository(persistenceService);

            var patients = GeneratePatients(patientCount);

            foreach (var patient in patients)
            {
                repo.AddPatient(patient.Name, patient.Birthday, patient.Alive, patient.ExternalId);
            }

            repo.PersistRepository();
			MessageBox.Show("fertig");
		}

        private static readonly Random Rand = new Random();


		private static IEnumerable<Patient> GeneratePatients (int count)
		{
			var firstNames = FirstNames();
			var surNames   = SurNames();

			var firstNamesCount = firstNames.Count;
			var surNamesCount = surNames.Count;


			var patients = new List<Patient>(count);			

			for (var i = 0; i < count; i++)
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


		private static void GenerateNamesList()
		{
			var firstNames = FirstNames();
			var surNames   = SurNames();			

			var firstNamesCount = firstNames.Count;
			var surNamesCount = surNames.Count;


			var nameFile = new StreamWriter("names.txt");

			nameFile.WriteLine("return new List<Patient>()");
			nameFile.WriteLine("{");

			for (var i = 0; i < 5000; i++)
			{
				nameFile.Write("\tnew Patient(\"" + 
							   firstNames[Rand.Next(firstNamesCount-1)] + " " + 
							   surNames[Rand.Next(surNamesCount-1)]  + "\", new Date(" + 
							   Rand.Next(1,28) + "," + 
							   Rand.Next(1,13) + "," + 
							   Rand.Next(1936, 1992) + "), " + 
							   GetRandomBoolValueAsString() + ", new Guid(), " + 
							   "\"" + i + "\"),");
				
				nameFile.WriteLine();
			}
			nameFile.WriteLine("};");

			nameFile.Close();
		}

		private static string GetRandomBoolValueAsString()
		{
			var d = Rand.NextDouble();

			return d < 0.2 ? "false" : "true";
		}

		private static bool GetRandomBoolValue()
		{	
			return Rand.NextDouble() > 0.2;
		}

		private static void SeperateFirstAndSurNames()
		{
			var names = NamesGeneratedByOnlineNameGenerator();

			ISet<string> vornamen = new SortedSet<string>();
			ISet<string> nachnamen = new SortedSet<string>();

			foreach (var name in names)
			{
				var split = name.Split(' ', '\t');

				if (split.Length == 2)
				{
					vornamen.Add(split[0]);
					nachnamen.Add(split[1]);
				}
				else
				{
					Console.WriteLine(name);
				}
			}

			IList<string> vornameList = vornamen.ToList();
			IList<string> nachnameList = nachnamen.ToList();

			var nameFile = new StreamWriter("names.txt");

			nameFile.WriteLine("return new List<string>()");
			nameFile.WriteLine("{");

			for (var i = 0; i < vornameList.Count; i += 10)
			{
				nameFile.Write("\t\"" + vornameList[i+0] + "\",");

				if (i+1 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+1] + "\",");
				if (i+2 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+2] + "\",");
				if (i+3 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+3] + "\",");
				if (i+4 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+4] + "\",");
				if (i+5 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+5] + "\",");
				if (i+6 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+6] + "\",");
				if (i+7 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+7] + "\",");
				if (i+8 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+8] + "\",");
				if (i+9 < vornameList.Count) nameFile.Write("\t\"" + vornameList[i+9] + "\",");

				nameFile.WriteLine();
			}
			nameFile.WriteLine("};");
			
			nameFile.WriteLine();
			nameFile.WriteLine();
			nameFile.WriteLine();


			nameFile.WriteLine("return new List<string>()");
			nameFile.WriteLine("{");

			for (var i = 0; i < nachnameList.Count; i += 10)
			{
				nameFile.Write("\t\"" + nachnameList[i+0] + "\",");

				if (i+1 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+1] + "\",");
				if (i+2 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+2] + "\",");
				if (i+3 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+3] + "\",");
				if (i+4 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+4] + "\",");
				if (i+5 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+5] + "\",");
				if (i+6 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+6] + "\",");
				if (i+7 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+7] + "\",");
				if (i+8 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+8] + "\",");
				if (i+9 < nachnameList.Count) nameFile.Write("\t\"" + nachnameList[i+9] + "\",");

				nameFile.WriteLine();
			}
			nameFile.WriteLine("};");


			nameFile.Close();
		}	

		private static IReadOnlyList<string> NamesGeneratedByOnlineNameGenerator()
		{
			return new List<string>
			{
				"Julia Moore","Andrew	Washington", "Ruby	Parker", "Stephanie	Stewart", "Christopher	Smith", "Edward	Taylor", "Lois	Powell",
				"Jacqueline	Sanders","Frank	Cook", "Cheryl	Green", "Julie	Butler", "Linda	Henderson", "Joan	Anderson", "Sarah	Jones",
				"Thomas Gonzales", "Lawrence	Wood", "Phillip	Cooper", "Jeffrey	Russell", "Douglas	Campbell", "Anne	Edwards", "Janice	Gray", 
				"Harry Turner", "Lisa	Diaz", "Anthony	Wright", "Dennis	Jackson", "Joe	Peterson", "Craig	Hall", "Benjamin	Barnes",
				"Nicholas King", "Dorothy	Simmons", "Bobby	Griffin", "Patrick	Ramirez", "Matthew	Allen", "Annie	Murphy", "Deborah	Wilson",
				"Katherine Bryant"  , "Lori	Hernandez", "Tina	Long", "Bruce	Hill", "Evelyn	Sanchez", "Patricia	Adams", "Martha	Foster",
				"Maria Lopez", "Gregory	Alexander", "Donald	Rogers", "George	Bennett",  "Margaret	Reed", "Jessica	Cox", "Nancy	Clark",
				"Carol Thomas", "Samuel	Garcia", "Carlos	Mitchell", "Jeremy	Lee", "Raymond	Martin", "James	Phillips","Todd	Brown",
				"Howard Davis", "Christina	Coleman", "Mildred	Lewis", "Jonathan	Nelson", "Kathryn	Perry","Mark	Evans", "Joshua	Scott",
				"Charles Thompson","William	Hughes", "Randy	Howard", "Andrea	Williams", "Carl	Carter", "Michael	Jenkins","Walter	Torres",
				"Henry White","Diana	Martinez","Jean	Perez","Nicole	Price","Kathy	Brooks","Kelly	Harris","Bonnie	Ward","Jack	Johnson",
				"Jennifer Patterson","Chris	Bailey","Billy	Baker","Michelle	Gonzalez","Melissa	Morgan","Norma	Rodriguez","Frances	Kelly","Paul Roberts",
				"Eugene Young","Aaron	Rivera","Keith	Collins","Jason	Robinson","Marilyn	Morris","Kenneth	Watson","Rose	Bell","Clarence	Richardson",
				"Jane James","Sean	Ross","Steven	Walker","Anna	Flores","Aaron	Carter","Tina	Thomas","Juan	Ward","Cynthia	Miller",
				"Patrick Diaz","Nicholas	Powell","Raymond	Collins","Julie	Johnson","Martin	Harris","Robert	Bailey","Wanda	Peterson",
				"Walter Anderson","Judy	Kelly","Jeremy	Wilson","Frances	Morgan","Steve	Bell","Michelle	Campbell","Emily	Taylor",
				"Dennis Hall","Arthur	Long","Rose	Cook","Timothy	Scott","Brian	Sanchez","Deborah	Ross","Shirley	Barnes",
				"Michael Parker","Denise	Lewis","Nancy	Hernandez","Jesse	Young","Joseph	Bryant","Frank	Robinson","Ruth	Murphy",
				"Shawn Richardson","Linda	Allen","James	White","Kathryn	Griffin","Bruce	Washington","Barbara	Perry","Norma	Martin",
				"Irene Lee","Judith	Morris","Kelly	Henderson","Diane	Baker","Carlos	King","Jean	Lopez","Kimberly	Walker",
				"Paula Ramirez","Phillip	Martinez","Gary	Roberts","Phyllis	Miller","Victor	Gray","Keith	Wright","Scott	Rivera",
				"Adam Rodriguez","Annie	Reed","Amy	Rogers","Lisa	Simmons","Rebecca	Smith","Lois	Coleman","Debra	Cooper",
				"Sandra Sanders","Beverly	Wood","Jacqueline	Jenkins","Angela	Patterson","Carol	Williams","Nicole	Torres","Ann	Perez",
				"Bonnie Nelson","Steven	Turner","Maria	Evans","Carl	Flores","Christina	Hughes","Catherine	Foster","Joe	Hill",
				"Carolyn Butler","Craig	Alexander","Kenneth	Cox","Stephanie	Price","Daniel	Garcia","Laura	Thompson","Mildred	Gonzalez",
				"Marie Adams","Clarence	Edwards","Dorothy	Phillips","Bobby	Howard","Wayne	Davis","Thomas	Stewart","Louise	Jackson",
				"Joshua Clark","Joyce	James","Ryan	Gonzales","Alan	Moore","Benjamin	Green","Elizabeth	Russell","William	Mitchell",
				"Louis Watson","Fred	Brown","Ernest	Brooks","Jerry	Jones","Eugene	Bennett","Debra	Cook","Michael	Bailey",
				"Phillip Howard","Pamela	Stewart","Michelle	Bell","Nicholas	Powell","Brandon	Allen","Harry	Sanchez","Phyllis	Adams",
				"Harold Washington","Deborah	Baker","Willie	Butler","Scott	Ross","Justin	Watson","Kathryn	Green","Carl	Bennett",
				"Lawrence Williams","Ronald	Hill","William	Jackson","Louis	Johnson","Jack	Phillips","Sara	King","Peter	Gonzalez",
				"Emily James","Maria	Simmons","Laura	Thomas","Dorothy	Kelly","Bonnie	White","Larry	Bryant","Doris	Jones",
				"Daniel Diaz","Gloria	Wright","Joe	Evans","Charles	Peterson","Nancy	Robinson","Martha	Anderson","Terry	Martinez",
				"Ruth Miller","Lori	Morris","Jesse	Davis","Julia	Jenkins","Victor	Long","Shirley	Roberts","Barbara	Carter",
				"Todd Flores","Catherine	Turner","Jennifer	Sanders","Lois	Russell","Jerry	Walker","Ernest	Hall","Samuel	Edwards",
				"Joseph Clark","Jessica	Scott","Christina	Griffin","Stephen	Gonzales","Kathleen	Rogers","Edward	Barnes","Fred	Rivera",
				"Christopher Wood","Kimberly	Thompson","Nicole	Moore","Amanda	Mitchell","Sharon	Torres","Norma	Perry","Jimmy	Parker",
				"Kelly Alexander","Walter	Campbell","Randy	Lee","Denise	Ramirez","Russell	Harris","Alan	Rodriguez","Jean	Collins",
				"Brenda Price","Jonathan	Gray","Evelyn	Martin","Mary	Reed","Angela	Wilson","Elizabeth	Cox","Rebecca	Hughes",
				"Craig Coleman","Jacqueline	Patterson","Kenneth	Morgan","Donald	Lopez","Matthew	Nelson","Cynthia	Henderson","David	Young",
				"Earl Murphy","Ralph	Taylor","Karen	Garcia","Rachel	Brown","Carlos	Perez","Kathy	Foster","Bobby	Richardson",
				"Theresa Brooks","Alice	Cooper","Wanda	Hernandez","Annie	Ward","Eric	Lewis","Joan	Smith","Joan	Carter",
				"Deborah Perry","Alice	Lopez","Gregory	Murphy","Susan	Bennett","Clarence	Martin","Judy	Jenkins","Nancy	Rodriguez",
				"Teresa Hughes","Joseph	Ward","Pamela	Stewart","Cheryl	Collins","Ralph	Turner","Diane	Richardson","Janet	Hernandez",
				"Philip Davis","Carl	Torres","Kenneth	White","Denise	Anderson","Matthew	King","Martha	Kelly","Scott	Campbell",
				"Albert Thompson","Sandra	Sanchez","Gerald	Lee","Jean	Reed","Irene	Parker","Christine	Ross","Michelle	Washington",
				"Alan Ramirez","Jessica	Henderson","Catherine	Rogers","Rachel	Cook","Cynthia	Nelson","Robert	Barnes","Andrea	James",
				"Larry Brooks","Douglas	Diaz","Gloria	Taylor","Rebecca	Cooper","Diana	Scott","James	Gray","Roy	Robinson",
				"Kathy Foster","Andrew	Jones","Sara	Hill","Amy	Lewis","Frank	Wood","Theresa	Wright","Julia	Perez",
				"Kelly Flores","Christina	Allen","Christopher	Bailey","Lisa	Bell","Stephanie	Young","Carlos	Alexander","Ryan	Johnson"				
			};
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

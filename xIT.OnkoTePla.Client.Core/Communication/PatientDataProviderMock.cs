using System;
using System.Collections.Generic;
using xIT.OnkoTePla.Contracts.Communication;
using xIT.OnkoTePla.Contracts.DataObjects;


namespace xIT.OnkoTePla.Client.Core.Communication
{
	public class PatientDataProviderMock : IPatientInfoProvider
	{
		public IReadOnlyList<Patient> GetPatients()
		{
			return new List<Patient>()
			{				
				new Patient("Marcel Käufer",      DateTime.Parse("1956-07-02"), true,   0),
				new Patient("Luca Dittmar",       DateTime.Parse("1985-05-13"), false,  1),
				new Patient("Thorsten Welter",    DateTime.Parse("1961-02-13"), true,   2),
				new Patient("Sigmund Bambach",    DateTime.Parse("1957-06-28"), true,   3),
				new Patient("Wilfried Weber",     DateTime.Parse("1962-09-30"), true,   4),
				new Patient("Dennis Keil",        DateTime.Parse("1966-02-28"), false,  5),
				new Patient("Hubertus Kröger",    DateTime.Parse("1989-05-02"), false,  6),
				new Patient("Leonhard Schmeling", DateTime.Parse("1959-02-26"), true,   7),
				new Patient("Philipp Frank",      DateTime.Parse("1964-02-21"), true,   8),
				new Patient("Lorenz Adenauer",    DateTime.Parse("1955-08-01"), true,   9),
				new Patient("Theophil Porsche",   DateTime.Parse("1984-05-28"), false, 10),
				new Patient("Thorsten Siegel",    DateTime.Parse("1970-03-03"), true,  11),
				new Patient("Ortwin Winther",     DateTime.Parse("1976-08-29"), false, 12),
				new Patient("Rupert Strand",      DateTime.Parse("1957-09-18"), true,  13),
				new Patient("Siegfried Tiedeman", DateTime.Parse("1990-04-15"), true,  14),
				new Patient("Hagen Shriver",      DateTime.Parse("1961-08-30"), false, 15),
				new Patient("Laurenz Abt",        DateTime.Parse("1955-11-16"), false, 16),
				new Patient("Justus Kruckel",     DateTime.Parse("1970-12-21"), false, 17),
				new Patient("Ludolf Beyersdorf",  DateTime.Parse("1961-02-24"), true,  18),
				new Patient("Dennis Keil",        DateTime.Parse("1967-11-16"), true,  19),
				new Patient("Hubertus Kröger",    DateTime.Parse("1955-09-15"), true,  20),
				new Patient("Leonhard Schmeling", DateTime.Parse("1979-07-26"), true,  21),
				new Patient("Philipp Frank",      DateTime.Parse("1959-09-03"), false, 22),
				new Patient("Lorenz Adenauer",    DateTime.Parse("1990-04-22"), true,  23)
			};
		}
	}
}

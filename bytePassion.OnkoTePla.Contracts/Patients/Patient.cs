using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Contracts.Patients
{
    [DataContract]
	public class Patient
	{
        [DataMember (Name = "Name")]
        private readonly string name;
        [DataMember(Name = "Alive")]
        private readonly bool   alive;
        [DataMember(Name = "Birthday")]
        private readonly Date   birthday;
        [DataMember(Name = "Id")]
        private readonly Guid   id;
        [DataMember(Name = "ExternalId")]
        private readonly string externalId;

		public Patient(string name, Date birthday, bool alive, 
					   Guid id, string externalId)
		{
			this.name     = name;
			this.alive    = alive;
			this.birthday = birthday; 
			this.id       = id;
			this.externalId = externalId;
		}

		public string Name       { get { return name;       }  }
        public bool   Alive      { get { return alive;      }  }
        public Date   Birthday   { get { return birthday;   } }
        public Guid   Id         { get { return id;         } }
        public string ExternalId { get { return externalId; } }


		#region ToString / HashCode / Equals

		public override string ToString ()
		{
			return Name + " (* " + Birthday + ")";
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (patient1, patient2) => patient1.Id == patient2.Id);
		}

		public override int GetHashCode ()
		{
			return id.GetHashCode();
		}

		#endregion

		public static IComparer<Patient> GetNameComparer()
		{
			return new PatientNameSorter();
		}

		private class PatientNameSorter : IComparer<Patient>
		{
			public int Compare(Patient x, Patient y)
			{
				return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
			}
		}
	}
}

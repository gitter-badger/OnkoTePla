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
		public static readonly Patient Dummy = new Patient("", Date.Dummy, false, new Guid(), "");

        [DataMember(Name = "Name")]       private readonly string name;
        [DataMember(Name = "Alive")]      private readonly bool   alive;
        [DataMember(Name = "Birthday")]   private readonly Date   birthday;
        [DataMember(Name = "Id")]         private readonly Guid   id;
        [DataMember(Name = "ExternalId")] private readonly string externalId;

		public Patient(string name, Date birthday, bool alive, 
					   Guid id, string externalId)
		{
			this.name     = name;
			this.alive    = alive;
			this.birthday = birthday; 
			this.id       = id;
			this.externalId = externalId;
		}

		public string Name       => name;
	    public bool   Alive      => alive;
	    public Date   Birthday   => birthday;
	    public Guid   Id         => id;
	    public string ExternalId => externalId;


		public override string ToString    ()           => $"{Name}(*{Birthday})";
	    public override bool   Equals      (object obj) => this.Equals(obj, (patient1, patient2) => patient1.Id == patient2.Id);
	    public override int    GetHashCode ()           => id.GetHashCode();


	    public static bool operator ==(Patient p1, Patient p2) => EqualsExtension.EqualsForEqualityOperator(p1, p2);
	    public static bool operator !=(Patient p1, Patient p2) => !(p1 == p2);

	    public static IComparer<Patient> GetNameComparer() => new PatientNameSorter();

	    private class PatientNameSorter : IComparer<Patient>
		{
			public int Compare(Patient x, Patient y)
			{
				return String.Compare(x.Name, y.Name, StringComparison.Ordinal);
			}
		}
	}
}

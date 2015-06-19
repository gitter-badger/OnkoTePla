using System;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Contracts.Patients
{

	public class Patient
	{
		private readonly string name;
		private readonly bool   alive;
		private readonly Date   birthday;
		private readonly Guid   id;
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

		public string Name       { get { return name;       }}		
		public bool   Alive      { get { return alive;      }}
		public Date   Birthday   { get { return birthday;   }}
		public Guid   Id         { get { return id;         }}
		public string ExternalId { get { return externalId; }}		
	}
}

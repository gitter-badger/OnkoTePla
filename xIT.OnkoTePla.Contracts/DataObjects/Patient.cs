using System;


namespace xIT.OnkoTePla.Contracts.DataObjects
{
	public sealed class Patient
	{
		private readonly string   name;
		private readonly bool     alive;
		private readonly DateTime birthday;
		private readonly uint     id;

		public Patient(string name, DateTime birthday, bool alive, uint id)
		{
			this.name     = name;
			this.alive    = alive;
			this.birthday = birthday; 
			this.id       = id;
		}

		public string   Name     { get { return name;     }}		
		public bool     Alive    { get { return alive;    }}
		public DateTime Birthday { get { return birthday; }}
		public uint     ID       { get { return id;       }}
	}
}

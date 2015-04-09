using System;


namespace xIT.OnkoTePla.Client.Core.DataObjects
{
	public sealed class Patient
	{
		private readonly string   _name;
		private readonly bool     _alive;
		private readonly DateTime _birthday;

		public Patient(string name, bool alive, DateTime birthday)
		{
			_name     = name;
			_alive    = alive;
			_birthday = birthday; 
		}

		public string   Name     { get { return _name;     }}
		public DateTime Birthday { get { return _birthday; }}
		public bool     Alive    { get { return _alive;    }}
	}
}

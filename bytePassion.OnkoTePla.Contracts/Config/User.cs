using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public class User
	{
		private readonly Guid   id;
		private readonly string name;
		private readonly IReadOnlyList<Guid> listOfAccessableMedicalPractices;
		private readonly string password;

		public User(string name, IReadOnlyList<Guid> listOfAccessableMedicalPractices, 
					string password, Guid id)
		{
			this.name = name;
			this.listOfAccessableMedicalPractices = listOfAccessableMedicalPractices;
			this.password = password;
			this.id = id;
		}

		public IReadOnlyList<Guid> ListOfAccessableMedicalPractices { get { return listOfAccessableMedicalPractices; }}
		public string              Name                             { get { return name;                             }}		
		public string              Password                         { get { return password;                         }}
		public Guid                Id                               { get { return id;                               }}

		#region ToString / HashCode / Equals

		public override string ToString ()
		{
			return Name;
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (user1, user2) => user1.Id.Equals(user2.Id));
		}

		public override int GetHashCode ()
		{
			return Id.GetHashCode();
		}

		#endregion
	}
}
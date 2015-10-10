using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public class User
	{
        public User(string name, IReadOnlyList<Guid> listOfAccessableMedicalPractices, 
					string password, Guid id)
		{
			Name = name;
			ListOfAccessableMedicalPractices = listOfAccessableMedicalPractices;
			Password = password;
			Id = id;
		}

		public IReadOnlyList<Guid> ListOfAccessableMedicalPractices { get; }
		public string              Name                             { get; }
		public string              Password                         { get; }
		public Guid                Id                               { get; }


		public override string ToString    ()           => Name;
	    public override bool   Equals      (object obj) => this.Equals(obj, (user1, user2) => user1.Id.Equals(user2.Id));
	    public override int    GetHashCode ()           => Id.GetHashCode();


		public static bool operator ==(User u1, User u2) => EqualsExtension.EqualsForEqualityOperator(u1, u2);
		public static bool operator !=(User u1, User u2) => !(u1 == u2);
	}
}
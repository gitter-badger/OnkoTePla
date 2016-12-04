using System;
using System.Collections.Generic;
using bytePassion.Lib.FrameworkExtensions;
using static bytePassion.Lib.FrameworkExtensions.EqualsExtension;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public class User
	{
        public User(string name, IReadOnlyList<Guid> listOfAccessableMedicalPractices, 
					string password, Guid id, bool isHidden)
		{
			Name = name;
			ListOfAccessableMedicalPractices = listOfAccessableMedicalPractices;
			Password = password;
			Id = id;
	        IsHidden = isHidden;
		}

		public IReadOnlyList<Guid> ListOfAccessableMedicalPractices { get; }

		public string Name     { get; }
		public string Password { get; }
		public Guid   Id       { get; }
		public bool   IsHidden { get; }

		public override string ToString ()
		{
			return Name;
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (user1, user2) => user1.Id == user2.Id &&
													  user1.Name == user2.Name &&
													  user1.Password == user2.Password &&
													  user1.IsHidden == user2.IsHidden &&
													  user1.ListOfAccessableMedicalPractices.Equals(user2.ListOfAccessableMedicalPractices));
		}

		public override int GetHashCode ()
		{
			return Id.GetHashCode() ^
				   Name.GetHashCode() ^
				   Password.GetHashCode() ^
				   IsHidden.GetHashCode() ^
				   ListOfAccessableMedicalPractices.GetHashCode();
		}

		public static bool operator ==(User u1, User u2) => EqualsForEqualityOperator(u1, u2);
		public static bool operator !=(User u1, User u2) => !(u1 == u2);
	}
}
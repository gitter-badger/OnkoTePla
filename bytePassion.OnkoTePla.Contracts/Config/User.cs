using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using bytePassion.Lib.FrameworkExtensions;

namespace bytePassion.OnkoTePla.Contracts.Config
{
    [DataContract]
	public class User
	{
        [DataMember(Name = "Id")]                               private readonly Guid                id;
        [DataMember(Name = "Name")]                             private readonly string              name;
        [DataMember(Name = "ListOfAccessableMedicalPractices")] private readonly IReadOnlyList<Guid> listOfAccessableMedicalPractices;
        [DataMember(Name = "Password")]                         private readonly string              password;

		public User(string name, IReadOnlyList<Guid> listOfAccessableMedicalPractices, 
					string password, Guid id)
		{
			this.name = name;
			this.listOfAccessableMedicalPractices = listOfAccessableMedicalPractices;
			this.password = password;
			this.id = id;
		}

		public IReadOnlyList<Guid> ListOfAccessableMedicalPractices => listOfAccessableMedicalPractices;
	    public string              Name                             => name;
	    public string              Password                         => password;
	    public Guid                Id                               => id;


		public override string ToString    ()           => Name;
	    public override bool   Equals      (object obj) => this.Equals(obj, (user1, user2) => user1.Id.Equals(user2.Id));
	    public override int    GetHashCode ()           => Id.GetHashCode();

	}
}
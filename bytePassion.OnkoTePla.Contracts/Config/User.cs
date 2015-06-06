using System;
using System.Collections.Generic;

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
	}
}
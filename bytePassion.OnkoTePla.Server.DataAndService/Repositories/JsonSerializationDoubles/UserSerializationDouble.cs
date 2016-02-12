using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JsonSerializationDoubles
{
	public class UserSerializationDouble
	{
		public UserSerializationDouble()
		{			
		}

		public UserSerializationDouble(User user)
		{
			ListOfAccessableMedicalPractices = user.ListOfAccessableMedicalPractices;
			Name     = user.Name;
			Password = user.Password;
			Id       = user.Id;
		}

		public IReadOnlyList<Guid> ListOfAccessableMedicalPractices { get; set; }

		public string Name     { get; set; }
		public string Password { get; set; }
		public Guid   Id       { get; set; }
		public bool   IsHidden { get; set; }

		public User GetUser()
		{
			return new User(Name, ListOfAccessableMedicalPractices, Password, Id, IsHidden);
		}
	}
}
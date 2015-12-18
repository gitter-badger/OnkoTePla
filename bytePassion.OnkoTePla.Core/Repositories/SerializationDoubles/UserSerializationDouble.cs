﻿using bytePassion.OnkoTePla.Contracts.Config;
using System;
using System.Collections.Generic;


namespace bytePassion.OnkoTePla.Core.Repositories.SerializationDoubles
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
		public string              Name                             { get; set; }
		public string              Password                         { get; set; }
		public Guid                Id                               { get; set; }

		public User GetUser()
		{
			return new User(Name, ListOfAccessableMedicalPractices, Password, Id);
		}
	}
}
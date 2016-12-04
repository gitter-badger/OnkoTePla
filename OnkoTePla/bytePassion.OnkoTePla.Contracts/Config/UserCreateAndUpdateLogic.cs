using System;
using System.Collections.Generic;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public static class UserCreateAndUpdateLogic
	{
		public static User Create()
		{
			return new User("noUserName", new List<Guid>(), "", Guid.NewGuid(), false);
		}

		public static User SetNewName(this User user, string newUserName)
		{
			return new User(newUserName,
							user.ListOfAccessableMedicalPractices,
							user.Password,
							user.Id,
							user.IsHidden);
		}

		public static User UpdateListOfAccessableMedicalPractices(this User user, IReadOnlyList<Guid> newListOfAccessableMedicalPractices)
		{
			return new User(user.Name,
							newListOfAccessableMedicalPractices,
							user.Password,
							user.Id,
							user.IsHidden);
		}

		public static User SetNewPassword(this User user, string newPassword)
		{
			return new User(user.Name,
							user.ListOfAccessableMedicalPractices,
							newPassword,
							user.Id,
							user.IsHidden);
		}

		public static User SetHiddenStatus(this User user, bool newIsHiddenStatus)
		{
			return new User(user.Name,
							user.ListOfAccessableMedicalPractices,
							user.Password,
							user.Id,
							newIsHiddenStatus);
		}

		public static User SetNewUserValues(this User user, 
										    string newUserName, string newPassword,
											IReadOnlyList<Guid> newListOfAccessableMedicalPractices, 
											bool newIsHiddenStatus)
		{
			return user.SetNewName(newUserName)
					   .SetNewPassword(newPassword)
					   .UpdateListOfAccessableMedicalPractices(newListOfAccessableMedicalPractices)
					   .SetHiddenStatus(newIsHiddenStatus);
		}
	}
}
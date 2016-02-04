using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetUserListResponse : NetworkMessageBase
	{
		public GetUserListResponse(IReadOnlyList<ClientUserData> availableUsers) 
			: base(NetworkMessageType.GetUserListResponse)
		{
			AvailableUsers = availableUsers;
		}

		public IReadOnlyList<ClientUserData> AvailableUsers { get; } 

		public override string AsString()
		{
			if (AvailableUsers.Count == 0)
				return "";

			var msgBuilder = new StringBuilder();

			foreach (var user in AvailableUsers)
			{
				msgBuilder.Append(user.Name);
				msgBuilder.Append(",");
				msgBuilder.Append(user.Id);
				msgBuilder.Append(";");
			}

			return msgBuilder.ToString().Substring(0, msgBuilder.Length - 1);
		}

		public static GetUserListResponse Parse(string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return new GetUserListResponse(new List<ClientUserData>());

			var userList = s.Split(';')
							.Select(element => element.Trim())							
							.Select(userInfo => userInfo.Split(',').ToList())
							.Select(userInfoParts => new ClientUserData(userInfoParts[0], Guid.Parse(userInfoParts[1])))
							.ToList();

			return new GetUserListResponse(userList);
		}
	}
}

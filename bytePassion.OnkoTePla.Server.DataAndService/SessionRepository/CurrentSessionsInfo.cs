using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection;

namespace bytePassion.OnkoTePla.Server.DataAndService.SessionRepository
{
	internal class CurrentSessionsInfo : ICurrentSessionsInfo
	{
		public event Action<SessionInfo> NewSessionStarted;
		public event Action<SessionInfo> SessionTerminated;
		public event Action<SessionInfo> LoggedInUserUpdated;

		private readonly IDictionary<ConnectionSessionId, SessionInfo> currentSessions; 

		public CurrentSessionsInfo()
		{
			currentSessions = new Dictionary<ConnectionSessionId, SessionInfo>();
		}

		public SessionInfo GetSessionInfo(ConnectionSessionId id)
		{
			lock (currentSessions)
			{
				return currentSessions.ContainsKey(id) 
					? currentSessions[id]
					: null;
			}
		}

		public bool DoesSessionExist(ConnectionSessionId id)
		{
			lock (currentSessions)
			{
				return currentSessions.ContainsKey(id);
			}
		}

		public bool IsUserLoggedIn(Guid userId)
		{
			lock (currentSessions)
			{
				return currentSessions.Values.Where(sessionInfo => sessionInfo.LoggedInUser != null)
											 .Any(sessionInfo => sessionInfo.LoggedInUser.Id == userId);
			}
		}

		public SessionInfo GetSessionForUser(Guid userId)
		{
			lock (currentSessions)
			{
				return currentSessions.Values.Where(sessionInfo => sessionInfo.LoggedInUser != null)
											 .FirstOrDefault(sessionInfo => sessionInfo.LoggedInUser.Id == userId);
			}
		}

		public void AddSession(ConnectionSessionId sessionId, Time creationTime, 
							   AddressIdentifier clientAddress, bool isDebugConnection)
		{
			lock (currentSessions)
			{
				var newSessionInfo = new SessionInfo(sessionId, creationTime, clientAddress, isDebugConnection);
				currentSessions.Add(sessionId, newSessionInfo);
				NewSessionStarted?.Invoke(newSessionInfo);
			}
		}

		public void RemoveSession(ConnectionSessionId sessionId)
		{
			lock (currentSessions)
			{
				var sessionToRemove = GetSessionInfo(sessionId);
				currentSessions.Remove(sessionId);
				SessionTerminated?.Invoke(sessionToRemove);
			}
		}

		public void UpdateLoggedInUser(ConnectionSessionId sessionId, User newUser)
		{
			lock (currentSessions)
			{
				var sessionToUpdate = GetSessionInfo(sessionId);
				currentSessions.Remove(sessionId);
				var updatedSessionInfo = new SessionInfo(sessionToUpdate.SessionId,
														 sessionToUpdate.CreationTime,
														 sessionToUpdate.ClientAddress,
														 sessionToUpdate.IsDebugConnection,
														 newUser);
				currentSessions.Add(sessionId, updatedSessionInfo);
				LoggedInUserUpdated?.Invoke(updatedSessionInfo);
			}
		}
	}
}

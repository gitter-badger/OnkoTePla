using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Locking;
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

		private readonly IList<Lock> locks;
		private readonly IDictionary<Lock, Timer> releaseLocksTimers; 

		public CurrentSessionsInfo()
		{
			currentSessions = new Dictionary<ConnectionSessionId, SessionInfo>();
			locks = new List<Lock>();
			releaseLocksTimers = new Dictionary<Lock, Timer>();
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

		public bool IsClientAddressConnected(AddressIdentifier clientAddress)
		{
			lock (currentSessions)
			{
				return currentSessions.Values.Any(sessionInfo => sessionInfo.ClientAddress == clientAddress);
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

				System.Windows.Application.Current.Dispatcher.Invoke(() =>
				{
					NewSessionStarted?.Invoke(newSessionInfo);
				});				
			}
		}

		public void RemoveSession(ConnectionSessionId sessionId)
		{
			lock (currentSessions)
			{
				var sessionToRemove = GetSessionInfo(sessionId);
				currentSessions.Remove(sessionId);

				System.Windows.Application.Current.Dispatcher.Invoke(() =>
				{
					SessionTerminated?.Invoke(sessionToRemove);
				});				
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

				System.Windows.Application.Current.Dispatcher.Invoke(() =>
				{
					LoggedInUserUpdated?.Invoke(updatedSessionInfo);
				});				
			}
		}

		public bool TryToGetLock(Guid medicalPracticeId, Date day)
		{
			lock (locks)
			{
				var potentialNewLock = new Lock(medicalPracticeId, day);

				if (locks.Any(reservedLock => Equals(reservedLock, potentialNewLock)))
				{					
					return false;
				}

				locks.Add(potentialNewLock);

				var newTimer = new Timer(TimerTick, 
										 potentialNewLock, 
										 TimeSpan.FromSeconds(3), 
										 TimeSpan.FromSeconds(3));
				
				releaseLocksTimers.Add(potentialNewLock, newTimer);				
				return true;				
			}
		}

		private void TimerTick(object observedLock)
		{
			var lockToRemove = (Lock) observedLock;
			StopTimerAndRemove(lockToRemove);

			lock (locks)
			{
				if (locks.Contains(lockToRemove))
				{					
					locks.Remove(lockToRemove);
				}
			}
		}

		private void StopTimerAndRemove(Lock @lock)
		{
			if (releaseLocksTimers.ContainsKey(@lock))
			{
				var timer = releaseLocksTimers[@lock];

				timer.Change(Timeout.Infinite, Timeout.Infinite);
				timer.Dispose();

				releaseLocksTimers.Remove(@lock);
			}
		}

		public void ReleaseLock(Guid medicalPracticeId, Date day)
		{
			lock (locks)
			{
				var lockToRemove = locks.FirstOrDefault(reservedLock => reservedLock.Day == day &&
				                                                        reservedLock.MedicalPracticeId == medicalPracticeId);

				if (lockToRemove != null)
				{
					StopTimerAndRemove(lockToRemove);
					locks.Remove(lockToRemove);					
				}
			}
		}

		public void ClearRepository()
		{
			lock (currentSessions)
			{
				foreach (var sessionId in currentSessions.Keys.ToList())
				{
					RemoveSession(sessionId);
				}
			}
		}
	}
}

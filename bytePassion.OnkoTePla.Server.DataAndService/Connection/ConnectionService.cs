using System;
using System.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.ResponseHandling;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.ThreadCollections;
using bytePassion.OnkoTePla.Server.DataAndService.Connection.Threads;
using bytePassion.OnkoTePla.Server.DataAndService.Data;
using bytePassion.OnkoTePla.Server.DataAndService.Repositories.EventStore;
using bytePassion.OnkoTePla.Server.DataAndService.SessionRepository;
using NetMQ;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection
{
	public class ConnectionService : DisposingObject, 
								     IConnectionService
	{		
		public event Action<SessionInfo> NewSessionStarted
		{
			add    { sessionRepository.NewSessionStarted += value; }
			remove { sessionRepository.NewSessionStarted -= value; }
		}
		public event Action<SessionInfo> SessionTerminated
		{
			add    { sessionRepository.SessionTerminated += value; }
			remove { sessionRepository.SessionTerminated -= value; }
		}
		public event Action<SessionInfo> LoggedInUserUpdated
		{
			add    { sessionRepository.LoggedInUserUpdated += value; }
			remove { sessionRepository.LoggedInUserUpdated -= value; }
		}
		 
		public SessionInfo GetSessionInfo (ConnectionSessionId id)
		{
			return sessionRepository.GetSessionInfo(id);
		}

		private readonly NetMQContext zmqContext;
		private readonly IDataCenter dataCenter;
		private readonly IEventStore eventStore;

		private readonly ICurrentSessionsInfo          sessionRepository;		
		private          IHeartbeatThreadCollection    heartbeatThreadCollection;
		private			 INotificationThreadCollection notificationThreadCollection;

		private UniversalResponseThread   universalResponseThread;
				
		
		internal ConnectionService (NetMQContext zmqContext, 
									IDataCenter dataCenter, 
									IEventStore eventStore) 								   
		{
			sessionRepository = new CurrentSessionsInfo();

			this.zmqContext = zmqContext;
			this.dataCenter = dataCenter;
			this.eventStore = eventStore;			
		}
		
		public void InitiateCommunication(Address serverAddress)
		{			
			heartbeatThreadCollection = new HeartbeatThreadCollection(zmqContext);
			heartbeatThreadCollection.ClientVanished += OnClientVanished;

			notificationThreadCollection = new NotificationThreadCollection(zmqContext);

			var responseHandlerFactory = new ResponseHandlerFactory(dataCenter, 
																	eventStore,
																	sessionRepository, 
																	NewConnectionEstablished, 
																	ConnectionEnded);

			universalResponseThread = new UniversalResponseThread(zmqContext, serverAddress, responseHandlerFactory);
			new Thread(universalResponseThread.Run).Start();
		}

		private void OnClientVanished(ConnectionSessionId connectionSessionId)
		{
			if (sessionRepository.DoesSessionExist(connectionSessionId))
			{                                                               //	the session does not exist if it was
				sessionRepository.RemoveSession(connectionSessionId);       //  ended corretly by connectionEndMessage
			}

			ConnectionEnded(connectionSessionId);
		}

		private void ConnectionEnded(ConnectionSessionId connectionSessionId)
		{
			heartbeatThreadCollection.StopThread(connectionSessionId);
			notificationThreadCollection.StopThread(connectionSessionId);
		}

		private void NewConnectionEstablished(AddressIdentifier clientAddress, ConnectionSessionId connectionSessionId)
		{
			heartbeatThreadCollection.AddThread(clientAddress, connectionSessionId);
			notificationThreadCollection.AddThread(clientAddress, connectionSessionId);
		}


		public void StopCommunication()
		{	
			sessionRepository.ClearRepository();

			if (heartbeatThreadCollection != null)
			{
				heartbeatThreadCollection.Dispose();
				heartbeatThreadCollection.ClientVanished -= OnClientVanished;
				heartbeatThreadCollection = null;
			}			

			notificationThreadCollection?.Dispose();
			notificationThreadCollection = null;

			universalResponseThread?.Stop();
			universalResponseThread = null;
		}

		public void SendEventNotification                  (DomainEvent      domainEvent)             { notificationThreadCollection.SendNotification(new EventNotificationObject                 (domainEvent));             }
		public void SendPatientAddedNotification           (Patient          newPatient)              { notificationThreadCollection.SendNotification(new PatientAddedNotificationObject          (newPatient));              }
		public void SendPatientUpdatedNotification         (Patient          updatedPatient)          { notificationThreadCollection.SendNotification(new PatientUpdatedNotificationObject        (updatedPatient));          }
		public void SendTherapyPlaceTypeAddedNotification  (TherapyPlaceType newTherapyPlaceType)     { notificationThreadCollection.SendNotification(new TherpyPlaceTypeAddedNotificationObject  (newTherapyPlaceType));     }
		public void SendTherapyPlaceTypeUpdatedNotification(TherapyPlaceType updatedTherapyPlaceType) { notificationThreadCollection.SendNotification(new TherpyPlaceTypeUpdatedNotificationObject(updatedTherapyPlaceType)); }

		protected override void CleanUp()
		{	
			StopCommunication();		
		}
	}
}
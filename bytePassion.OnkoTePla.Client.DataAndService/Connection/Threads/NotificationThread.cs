using System;
using bytePassion.Lib.ConcurrencyLib;
using bytePassion.Lib.Types.Communication;
using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Communication.NetworkMessages.Notifications;
using bytePassion.OnkoTePla.Communication.SendReceive;
using bytePassion.OnkoTePla.Contracts.Domain.Events.Base;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;
using bytePassion.OnkoTePla.Contracts.Types;
using bytePassion.OnkoTePla.Resources;
using NetMQ;

namespace bytePassion.OnkoTePla.Client.DataAndService.Connection.Threads
{
	internal class NotificationThread : IThread
	{
		public event Action<DomainEvent> NewDomainEventAvailable;
		public event Action<Patient> NewPatientAvailable;
		public event Action<Patient> UpdatedPatientAvailable;
		public event Action<TherapyPlaceType> NewTherapyPlaceTypeAvailable;
		public event Action<TherapyPlaceType> UpdatedTherapyPlaceTypeAvailable;

		private readonly NetMQContext context;
		private readonly Address clientAddress;
		private readonly ConnectionSessionId sessionId;

		private volatile bool stopRunning;

		public NotificationThread (NetMQContext context,
								   Address clientAddress,
								   ConnectionSessionId sessionId)
		{
			this.context = context;
			this.clientAddress = clientAddress;
			this.sessionId = sessionId;

			IsRunning = true;
			stopRunning = false;
		}

		public void Run ()
		{
			using (var socket = context.CreatePullSocket())
			{
				socket.Bind(clientAddress.ZmqAddress + ":" + GlobalConstants.TcpIpPort.Notification);
				
				while (!stopRunning)
				{
					var notification = socket.ReceiveNetworkMsg(TimeSpan.FromSeconds(1));

					if (notification == null)										
						continue;

					switch (notification.Type)
					{
						case NetworkMessageType.EventBusNotification:
						{
							var eventNotification = (EventBusNotification) notification;

							if (eventNotification.SessionId == sessionId)
								NewDomainEventAvailable(eventNotification.NewEvent);

							break;
						}
						case NetworkMessageType.PatientAddedNotification:
						{
							var patientAddedNotification = (PatientAddedNotification) notification;

							if (patientAddedNotification.SessionId == sessionId)
								NewPatientAvailable(patientAddedNotification.Patient);
													
							break;
						}
						case NetworkMessageType.PatientUpdatedNotification:
						{
							var patientUpdatedNotification = (PatientAddedNotification) notification;

							if (patientUpdatedNotification.SessionId == sessionId)
								UpdatedPatientAvailable(patientUpdatedNotification.Patient);

							break;
						}
						case NetworkMessageType.TherapyPlaceTypeAddedNotification:
						{
							var therpyPlaceTypeAddedNotification = (TherapyPlaceTypeAddedNotification) notification;

							if (therpyPlaceTypeAddedNotification.SessionId == sessionId)
								NewTherapyPlaceTypeAvailable(therpyPlaceTypeAddedNotification.TherapyPlaceType);
													
							break;
						}
						case NetworkMessageType.TherapyPlaceTypeUpdatedNotification:
						{
							var therpyPlaceTypeUpdatedNotification = (TherapyPlaceTypeUpdatedNotification) notification;

							if (therpyPlaceTypeUpdatedNotification.SessionId == sessionId)
								UpdatedTherapyPlaceTypeAvailable(therpyPlaceTypeUpdatedNotification.TherapyPlaceType);

							break;
						}

						default:
							throw new ArgumentException();
					}
				}
			}
		}

		public void Stop ()
		{
			stopRunning = true;
		}

		public bool IsRunning { get; }
	}
}
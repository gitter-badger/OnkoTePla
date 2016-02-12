using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal class PatientUpdatedNotificationObject : NotificationObject
	{
		public PatientUpdatedNotificationObject (Patient patient)
			: base(NetworkMessageType.PatientUpdatedNotification)
		{
			Patient = patient;
		}

		public Patient Patient { get; }
	}
}
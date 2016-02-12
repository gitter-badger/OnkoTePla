using bytePassion.OnkoTePla.Communication.NetworkMessages;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Server.DataAndService.Connection.NotificationObjects
{
	internal class PatientAddedNotificationObject : NotificationObject
	{
		public PatientAddedNotificationObject(Patient patient)
			: base(NetworkMessageType.PatientAddedNotification)
		{
			Patient = patient;
		}
		
		public Patient Patient { get; }
	}
}
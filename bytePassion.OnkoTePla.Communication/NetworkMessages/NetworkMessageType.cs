namespace bytePassion.OnkoTePla.Communication.NetworkMessages
{
	public enum NetworkMessageType
	{
		HeartbeatRequest,
		HeartbeatResponse,
		BeginConnectionRequest,
		BeginConnectionResponse,
		BeginDebugConnectionRequest,
		BeginDebugConnectionResponse,
		EndConnectionRequest,
		EndConnectionResponse,
		GetUserListRequest,		
		GetUserListResponse,		
		LoginRequest,
		LoginResponse,
		LogoutRequest,
		LogoutResponse,
		GetAccessablePracticesRequest,
		GetAccessablePracticesResponse,
		GetPatientListRequest,
		GetPatientListResponse,
		GetAppointmentsOfADayRequest,
		GetAppointmentsOfADayResponse,
		GetAppointmentsOfAPatientRequest,
		GetAppointmentsOfAPatientResponse,
		GetMedicalPracticeRequest,
		GetMedicalPracticeResponse,
		GetTherapyPlacesTypeListRequest,
		GetTherapyPlacesTypeListResponse,
		GetPracticeVersionInfoRequest,
		GetPracticeVersionInfoResponse,
		TryToAddNewEventsRequest,
		TryToAddNewEventsResponse,
		
		EventBusNotification,
		PatientAddedNotification,
		PatientUpdatedNotification,		
		TherapyPlaceTypeAddedNotification,
		TherapyPlaceTypeUpdatedNotification,

		ErrorResponse,
		
	}
}

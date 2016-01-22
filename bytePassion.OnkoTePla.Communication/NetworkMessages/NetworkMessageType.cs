namespace bytePassion.OnkoTePla.Communication.NetworkMessages
{
	public enum NetworkMessageType
	{
		HeartbeatRequest,
		HeartbeatResponse,
		BeginConnectionRequest,
		BeginConnectionResponse,
		EndConnectionRequest,
		EndConnectionResponse,
		GetUserListRequest,		
		GetUserListResponse,		
		LoginRequest,
		LoginResponse,
		LogoutRequest,
		LogoutResponse,

		ErrorResponse
	}
}

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


		ErrorResponse		
	}
}

namespace bytePassion.OnkoTePla.Resources
{
	public static class GlobalConstants
    {		
		public static class TcpIpPort
		{
			public const uint BeginConnection = 6656;
			public const uint RequestData     = 6657;
			public const uint EndConnection   = 6658;
			public const uint Login           = 6659;
			public const uint Logout          = 6660;
			public const uint Heartbeat       = 6661;
		}

		public const int HeartbeatIntverval					=  5000;	// unit is milliseconds
		public const int ClientWaitTimeForHeartbeat			= 10000;	//			"
		public const int ServerWaitTimeForHeartbeatResponse =  2000;    //			"

		public const string PatientPersistenceFile          = @"..\..\..\AppData\patients.xml";		
		public const string ConfigPersistenceFile           = @"..\..\..\AppData\config.xml";
		public const string EventHistoryPersistenceFile     = @"..\..\..\AppData\eventHistory.xml";

		public const string PatientJsonPersistenceFile      = @"..\..\..\AppData\patients.json";
		public const string ConfigJsonPersistenceFile       = @"..\..\..\AppData\config.json";		
		public const string EventHistoryJsonPersistenceFile = @"..\..\..\AppData\eventHistory.json";		
	}
}

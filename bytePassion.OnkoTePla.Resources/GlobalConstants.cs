using System;

namespace bytePassion.OnkoTePla.Resources
{
	public static class GlobalConstants
    {		
		public static class TcpIpPort
		{						 
			public const uint Heartbeat    = 6656;
			public const uint Request      = 6657;
			public const uint Notification = 6658;			
		}

		public static readonly char[] ForbiddenCharacters = {'|',';',',',':','.','#',
															 '(','[','{','}',']',')' };

		public const uint StandardSendingTimeout			 =  2000;    // unit is milliseconds
		public const uint HeartbeatIntverval				 =  5000;    //			"
		public const uint ClientWaitTimeForHeartbeat		 = 10000;	 //			"
		public const uint ServerWaitTimeForHeartbeatResponse =  2000;    //			"

		private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

		public static readonly string ClientBasePath = AppDataPath + @"\bytePassion\OnkoTePla\Client\";
		public static readonly string ServerBasePath = AppDataPath + @"\bytePassion\OnkoTePla\Server\";
		public static readonly string BackupBasePath = AppDataPath + @"\bytePassion\OnkoTePla\Backup\";
		
		public static readonly string PatientPersistenceFile       = ServerBasePath + @"patients.xml";		
		public static readonly string ConfigPersistenceFile        = ServerBasePath + @"config.xml";				
		public static readonly string MetaDataPersistanceFile      = ServerBasePath + @"metaData.xml";
		public static readonly string EventHistoryBasePath         = ServerBasePath + @"EventHistory";
		
		public static readonly string LocalSettingsPersistenceFile = ClientBasePath + @"settings.xml";
	}
}

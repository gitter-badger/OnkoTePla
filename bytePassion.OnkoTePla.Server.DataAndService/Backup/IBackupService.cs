namespace bytePassion.OnkoTePla.Server.DataAndService.Backup
{
	internal interface IBackupService
	{
		void Export(string filename);
		void Import(string filename);
	}
}

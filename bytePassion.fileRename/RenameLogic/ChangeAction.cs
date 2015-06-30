
using bytePassion.FileRename.RenameLogic.Enums;


namespace bytePassion.FileRename.RenameLogic
{
	public class ChangeAction
	{		
		private readonly string originalFileOrDirectoryName;
		private readonly string renamedFileOrDirectoryName;
		private readonly ItemType changedNameType;

		public ChangeAction (string originalFileOrDirectoryName,
							string renamedFileOrDirectoryName,
							ItemType changedNameType)
		{
			this.originalFileOrDirectoryName = originalFileOrDirectoryName;
			this.renamedFileOrDirectoryName = renamedFileOrDirectoryName;
			this.changedNameType = changedNameType;
		}

		public string OriginalFileOrDirectoryName { get { return originalFileOrDirectoryName; } }
		public string RenamedFileOrDirectoryName { get { return renamedFileOrDirectoryName; } }
		public ItemType ChangedNameType { get { return changedNameType; } }
	}
}

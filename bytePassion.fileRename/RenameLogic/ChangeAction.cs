
namespace bytePassion.FileRename.RenameLogic
{
	public class ChangeAction
	{
		public enum ChangeType { File, Directory }

		private readonly string originalFileOrDirectoryName;
		private readonly string renamedFileOrDirectoryName;
		private readonly ChangeType changedNameType;

		public ChangeAction (string originalFileOrDirectoryName,
							string renamedFileOrDirectoryName,
							ChangeType changedNameType)
		{
			this.originalFileOrDirectoryName = originalFileOrDirectoryName;
			this.renamedFileOrDirectoryName = renamedFileOrDirectoryName;
			this.changedNameType = changedNameType;
		}

		public string OriginalFileOrDirectoryName { get { return originalFileOrDirectoryName; } }
		public string RenamedFileOrDirectoryName { get { return renamedFileOrDirectoryName; } }
		public ChangeType ChangedNameType { get { return changedNameType; } }
	}
}

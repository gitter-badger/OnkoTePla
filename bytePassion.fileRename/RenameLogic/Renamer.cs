using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace bytePassion.FileRename.RenameLogic
{
	public class Renamer
	{
		

		private readonly Tuple<bool, string> abortReturn = new Tuple<bool, string>(false, "Manuell abgebrochen");

		private readonly DirectoryInfo startFolder;

		private readonly Func<string, bool>   nameIsMatchFunc;
		private readonly Func<string, string> fileReplaceFunc;
		private readonly Func<string, string> directoryReplaceFunc;
		
		private readonly bool includeSubfolders;
		private readonly bool changeFolderNames;

		private volatile bool abortSearch;

		private readonly IList<ChangeAction> changes;		

		public delegate void ItemProcessedEventHandler (string directory, string oldFileName, string newFilename);
		public delegate void ProcessFinishedEventHandler(bool finishedSuccessful, string errorMessage);

		public event ItemProcessedEventHandler ItemProcessed;
		public event ProcessFinishedEventHandler ProcessFinished;

		public Renamer(DirectoryInfo startFolder,
					   Func<string, bool> nameIsMatchFunc, 
					   Func<string, string> fileReplaceFunc, 
					   Func<string, string> directoryReplaceFunc, 
					   bool includeSubfolders, bool changeFolderNames)
		{
			this.startFolder = startFolder;
			this.nameIsMatchFunc = nameIsMatchFunc;			
			this.includeSubfolders = includeSubfolders;
			this.changeFolderNames = changeFolderNames;
			this.fileReplaceFunc = fileReplaceFunc;
			this.directoryReplaceFunc = directoryReplaceFunc;

			changes = new List<ChangeAction>();

			abortSearch = false;
			UndoAvailable = false;
		}

		public async void StartRenaming()
		{
			var result = await RecursiveRenamerAsync(startFolder);
			
			if (ProcessFinished != null) 
				ProcessFinished(result.Item1, result.Item2);
		}

		public void AbortRenaming()
		{
			abortSearch = true;
		}

		public bool UndoAvailable { get; private set; }

		public async void UndoRenaming()
		{
			UndoAvailable = false;

			var result = await UndoRenamingAsync();

			if (ProcessFinished != null)
				ProcessFinished(result.Item1, result.Item2);
		}

		private Task<Tuple<bool, string>> UndoRenamingAsync ()
		{
			return Task.Factory.StartNew(() =>
			{

				foreach (var changeAction in changes.Reverse())
				{
					if (changeAction.ChangedNameType == ChangeAction.ChangeType.File)
						File.Move(changeAction.RenamedFileOrDirectoryName, changeAction.OriginalFileOrDirectoryName);
					else
						Directory.Move(changeAction.RenamedFileOrDirectoryName, changeAction.OriginalFileOrDirectoryName);											
				}
				
				return new Tuple<bool, string>(true, "Rückgänigmachung erfolgreich");
			});
		}

		private void AddFileToLastChanges(string originalFileName, string renamedFileName)
		{
			changes.Add(new ChangeAction(originalFileName, renamedFileName, ChangeAction.ChangeType.File));			
			UndoAvailable = true;
		}

		private void AddDirectoryToLastChanges(string originalDirectoryName, string renamedDirectoryName)
		{
			changes.Add(new ChangeAction(originalDirectoryName, renamedDirectoryName, ChangeAction.ChangeType.Directory));		
			UndoAvailable = true;
		}

		private Task<Tuple<bool, string>> RecursiveRenamerAsync(DirectoryInfo directory)
		{
			return Task.Factory.StartNew(() => RecursiveRenamer(directory));
		}

		private Tuple<bool, string> RecursiveRenamer(DirectoryInfo directory)
		{
			try
			{
				if (abortSearch)
					return abortReturn;

				foreach (var currentFile in directory.GetFiles())
				{					
					if (abortSearch)
						return abortReturn;

					if (nameIsMatchFunc(currentFile.Name))
					{
						var oldName = currentFile.Name;
						var newName = fileReplaceFunc(currentFile.Name);


						currentFile.MoveTo(currentFile.DirectoryName + "\\" + newName);

						AddFileToLastChanges(currentFile.DirectoryName + "\\" + oldName,
											 currentFile.DirectoryName + "\\" + newName);

						if (ItemProcessed != null) 
							ItemProcessed(directory.FullName, oldName, newName);
					}
					else
					{
						if (ItemProcessed != null) 
							ItemProcessed(directory.FullName, currentFile.Name, "Name nicht geändert");
					}
				}

				if (includeSubfolders)
				{
					if (abortSearch)
						return abortReturn;

					foreach (DirectoryInfo currentDirectory in directory.GetDirectories())
					{
						var retVal = RecursiveRenamer(currentDirectory);

						if (!retVal.Item1)
							return retVal;
					}
				}

				if (abortSearch)
					return abortReturn;

				if (changeFolderNames)
					if (nameIsMatchFunc(directory.Name))
					{
						var oldName = directory.Name;
						var newName = directoryReplaceFunc(directory.Name);
						
						if (directory.Parent != null)
						{
							var fullOldName = directory.Parent.FullName + "\\" + oldName;
							var fullNewName = directory.Parent.FullName + "\\" + newName;

							directory.MoveTo(fullNewName);

							AddDirectoryToLastChanges(fullOldName, fullNewName);						
						}

						if (ItemProcessed != null)
							ItemProcessed(directory.FullName + "\t [Ordnername geändert]", oldName, newName);
					}				
			}
			catch (Exception except)
			{
				return new Tuple<bool, string>(false, except.Message);
			}
			return new Tuple<bool, string>(true, String.Empty);
		}
	}	
}

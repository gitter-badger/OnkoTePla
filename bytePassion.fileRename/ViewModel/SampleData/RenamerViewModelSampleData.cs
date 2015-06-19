using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.FileRename.Enums;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.FileRename.ViewModel.SampleData
{
	public class RenamerViewModelSampleData : IRenamerViewModel
	{
		public RenamerViewModelSampleData()
		{
			IsProcessStartable = true;
			IsProcessAbortable = false;
			IsProcessUndoable  = true;
			IsProcessRunning   = false;

			StartDirectory = "Ordner wählen ....";
			SearchParameterIncludeSubfolders = true;
			SearchParameterChangeFolderNames = true;			

			Columns = new ObservableCollection<ColumnDescriptor>()
			{
				new ColumnDescriptor { Header = "Aktueller Ordner", DisplayMember = FileListItem.CurrentDirectoryVariableName },
				new ColumnDescriptor { Header = "Alter Dateiname",  DisplayMember = FileListItem.OldFileNameVariableName      },
				new ColumnDescriptor { Header = "Neuer DateiName",  DisplayMember = FileListItem.NewFileNameVariableName      }
			};

			LastExecutedStartFolders = new ObservableCollection<string>
			                           {
				                           @"C:\test.txt",
										   @"D:\test2.txt"
			                           };
		}

		public ICommand Start            { get { return null; }}
		public ICommand Abort            { get { return null; }}
		public ICommand SelectFolder     { get { return null; }}
		public ICommand UndoLastRenaming { get { return null; }}

		public bool IsProcessStartable { get; set; }
		public bool IsProcessAbortable { get; set; }
		public bool IsProcessUndoable  { get; set; }
		public bool IsProcessRunning   { get; set; }					

		public string StartDirectory { get; set; }

		public string SearchString  { get; set; }
		public string ReplaceString { get; set; }

		public SearchType SearchType   { get; set; }
		public ReplaceType ReplaceType { get; set; }

		public bool SearchParameterCaseSensitivity   { get; set; }
		public bool SearchParameterIncludeSubfolders { get; set; }
		public bool SearchParameterChangeFolderNames { get; set; }

		public ObservableCollection<ColumnDescriptor> Columns   { get; set; }
		public ObservableCollection<FileListItem>     ListItems { get; set; }

		public ObservableCollection<string> LastExecutedStartFolders { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

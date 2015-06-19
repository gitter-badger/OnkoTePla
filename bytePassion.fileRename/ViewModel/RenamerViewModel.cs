using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using bytePassion.FileRename.Enums;
using bytePassion.FileRename.RenameLogic;
using bytePassion.Lib.Commands;
using bytePassion.Lib.FrameworkExtensions;
using Ookii.Dialogs.Wpf;


namespace bytePassion.FileRename.ViewModel
{
	public class RenamerViewModel : IRenamerViewModel
	{

		private Renamer renamer;				

		private SearchType  searchType;
		private ReplaceType replaceType;

		private string startDirectory;
		private string searchString;
		private string replaceWidthString;

		private bool searchParameterCaseSensitivity;
		private bool searchParameterIncludeSubfolers;
		private bool searchParameterChangeFolderNames;
		
		private readonly Command startCommand;
		private readonly Command abortCommand;
		private readonly Command selectFolderCommand;
		private readonly Command undoLastRenamingCommand;

		private readonly ObservableCollection<ColumnDescriptor> columns;
		private readonly ObservableCollection<FileListItem>     listItems;
		private readonly ObservableCollection<string>           lastExecutedStartFolders;

		private bool isProcessStartable;
		private bool isProcessAbortable;
		private bool isProcessUndoable;
		private bool isProcessRunning;
		


		public RenamerViewModel(IEnumerable<string> lastExecutedStartFolders)
		{			
			startCommand = new Command(
				() => { 

					SetUpRenamingProcess(); 
					listItems.Clear();
					renamer.StartRenaming();

					LastExecutedStartFolders.Add(StartDirectory);

					IsProcessStartable = false;
					IsProcessAbortable = true;
					IsProcessRunning = true;					
				},

				() => {

					if (!IsProcessStartable) return false;
					if (SearchType == SearchType.Characters && String.IsNullOrEmpty(SearchString)) return false;
					if (ReplaceType == ReplaceType.Characters && String.IsNullOrEmpty(ReplaceString)) return false;
					if (String.IsNullOrEmpty(StartDirectory)) return false;

					return true;
				}
			);

			abortCommand = new Command(
				() =>  renamer.AbortRenaming(),						
				() =>  IsProcessAbortable
			);

			selectFolderCommand = new Command(
				ShowDirectoryDialog,
				() => !IsProcessRunning
			);

			undoLastRenamingCommand = new Command(
				() => {

					renamer.UndoRenaming();
					IsProcessStartable = false;
					IsProcessAbortable = false;
					IsProcessRunning = true;
				},
				() => IsProcessUndoable
			);

			columns = new ObservableCollection<ColumnDescriptor>()
			{
				new ColumnDescriptor { Header = "Aktueller Ordner", DisplayMember = FileListItem.CurrentDirectoryVariableName },
				new ColumnDescriptor { Header = "Alter Dateiname",  DisplayMember = FileListItem.OldFileNameVariableName      },
				new ColumnDescriptor { Header = "Neuer DateiName",  DisplayMember = FileListItem.NewFileNameVariableName      }
			};

			listItems = new ObservableCollection<FileListItem>();
			this.lastExecutedStartFolders = new ObservableCollection<string>(lastExecutedStartFolders);

			IsProcessAbortable = false;
			IsProcessStartable = true;
			IsProcessRunning   = false;
			IsProcessUndoable  = false;

			SearchParameterCaseSensitivity  = false;
			SearchParameterIncludeSubfolders = true;
			SearchParameterChangeFolderNames = true;

			SearchType  = SearchType.WhiteSpace;
			ReplaceType = ReplaceType.Delete;
		}
		
		private void SetUpRenamingProcess()
		{
			renamer = new Renamer(new DirectoryInfo(StartDirectory), 
								  IsMatchFunc.Create(SearchType, SearchParameterCaseSensitivity, SearchString), 
								  ReplaceFunc.CreateFileReplaceFunc(SearchType, ReplaceType, SearchString, ReplaceString, SearchParameterCaseSensitivity),
								  ReplaceFunc.CreateDirectoryReplaceFunc(SearchType, ReplaceType, SearchString, ReplaceString, SearchParameterCaseSensitivity),
								  SearchParameterIncludeSubfolders, 
								  SearchParameterChangeFolderNames);

			renamer.ItemProcessed += (directory, oldFilename, newFilename) =>
			{
				Application.Current.Dispatcher.Invoke(() => ListItems.Add(new FileListItem(directory, oldFilename, newFilename)));
			};

			renamer.ProcessFinished += (successful, message) =>
			{
				StringBuilder builder = new StringBuilder();

				builder.Append(successful
					               ? "Der Prozess wurde erfolgreich beendet"
					               : "Der Prozess wurde mit folgender Meldung abgebrochen");

				if (!String.IsNullOrEmpty(message))
					builder.Append(":\n\n" + message);

				MessageBox.Show(builder.ToString());

				IsProcessUndoable = renamer.UndoAvailable;
				IsProcessStartable = true;
				IsProcessAbortable = false;
				IsProcessRunning = false;

				StartDirectory = StartDirectory; // to check weather current folder is still available
			};
		}
	

		public ICommand Start            { get { return startCommand;            }}
		public ICommand Abort            { get { return abortCommand;            }}
		public ICommand SelectFolder     { get { return selectFolderCommand;     }}
		public ICommand UndoLastRenaming { get { return undoLastRenamingCommand; }}


		public bool IsProcessRunning
		{
			get { return isProcessRunning;}
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref isProcessRunning, value);
				selectFolderCommand.RaiseCanExecuteChanged();
			}
		}

		public bool IsProcessStartable
		{
			get { return isProcessStartable; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref isProcessStartable, value);
				startCommand.RaiseCanExecuteChanged();				
			}
		}

		public bool IsProcessAbortable
		{
			get { return isProcessAbortable; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref isProcessAbortable, value);
				abortCommand.RaiseCanExecuteChanged();
			}
		}

		public bool IsProcessUndoable
		{
			get { return isProcessUndoable; }
			private set
			{
				PropertyChanged.ChangeAndNotify(this, ref isProcessUndoable, value);
				undoLastRenamingCommand.RaiseCanExecuteChanged();
			}
		}

		private void ShowDirectoryDialog()
		{
			var dialog = new VistaFolderBrowserDialog
			{
				Description = @"Bitte Ordner Auswählen",
				UseDescriptionForTitle = true
			};
			
			var showDialog = dialog.ShowDialog(null);
			if (showDialog != null && (bool)showDialog)
				StartDirectory = dialog.SelectedPath;
		}		

		public string StartDirectory
		{
			get {return startDirectory; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref startDirectory, value);

				IsProcessStartable = Directory.Exists(StartDirectory);

				startCommand.RaiseCanExecuteChanged();
			}
		}

		public string SearchString
		{
			get { return searchString; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref searchString, value);
				startCommand.RaiseCanExecuteChanged();
			}
		}

		public string ReplaceString
		{
			get { return replaceWidthString; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref replaceWidthString, value);
				startCommand.RaiseCanExecuteChanged();
			}
		}

		public SearchType SearchType
		{
			get { return searchType; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref searchType, value);
				startCommand.RaiseCanExecuteChanged();
			}
		}

		public ReplaceType ReplaceType 
		{
			get { return replaceType; }
			set
			{
				PropertyChanged.ChangeAndNotify(this, ref replaceType, value);
				startCommand.RaiseCanExecuteChanged();
			} 
		}

		public bool SearchParameterCaseSensitivity
		{
			get { return searchParameterCaseSensitivity; }
			set { PropertyChanged.ChangeAndNotify(this, ref searchParameterCaseSensitivity, value); }
		}

		public bool SearchParameterIncludeSubfolders
		{
			get { return searchParameterIncludeSubfolers; }
			set { PropertyChanged.ChangeAndNotify(this, ref searchParameterIncludeSubfolers, value); }
		}

		public bool SearchParameterChangeFolderNames
		{
			get { return searchParameterChangeFolderNames; }
			set { PropertyChanged.ChangeAndNotify(this, ref searchParameterChangeFolderNames, value); }
		}

		public ObservableCollection<ColumnDescriptor> Columns                  { get { return columns;                  }}
		public ObservableCollection<FileListItem>     ListItems                { get { return listItems;                }}
		public ObservableCollection<string>           LastExecutedStartFolders { get { return lastExecutedStartFolders; }}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

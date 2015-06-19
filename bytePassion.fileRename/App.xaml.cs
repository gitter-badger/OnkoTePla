using System.Windows;
using bytePassion.FileRename.Repository;
using bytePassion.FileRename.ViewModel;


namespace bytePassion.FileRename {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			var xmlStringDataStore = new XmlStringDataStore("lastProcessedFolders.xml");
			ILastUsedStartFoldersRepository repo = new LastUsedStartFoldersRepository(xmlStringDataStore);
			repo.LoadFromXml();
			var readModel = new RenamerViewModel(repo.GetAll());

			var mainWindow = new MainWindow()
			{
				DataContext = readModel
			};

			mainWindow.ShowDialog();

			repo.Add(readModel.LastExecutedStartFolders);
			repo.SaveToXml();
		}		
	}
}

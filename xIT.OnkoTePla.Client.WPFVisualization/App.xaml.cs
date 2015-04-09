using System.Windows;


namespace xIT.OnkoTePla.Client.WPFVisualization
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			// TODO: Composition Root here !!


			var mainWindow = new MainWindow();
			mainWindow.Show();
		}
	}
}

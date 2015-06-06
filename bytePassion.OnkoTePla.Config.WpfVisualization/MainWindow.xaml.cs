using System.Windows;
using bytePassion.OnkoTePla.Config.WpfVisualization.SampleData;


namespace bytePassion.OnkoTePla.Config.WpfClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow ()
		{
			InitializeComponent();
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			ConfigurationData.ConfigToXml();
		}

		private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
		{
			ConfigurationData.TestLoad();	
		}
	}
}

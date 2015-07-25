using System.Windows;
using bytePassion.OnkoTePla.Config.WpfVisualization.SampleData;
using bytePassion.OnkoTePla.Config.WpfVisualization.Test;


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

		private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
		{
			ConfigurationData.ConfigToJson();
		}

		private void ButtonBase_OnClick2(object sender, RoutedEventArgs e)
		{
			ConfigurationData.TestLoad();	
		}

		private void ButtonBase_OnClick3(object sender, RoutedEventArgs e)
		{
			EventStreamDataBase.GenerateExampleEventStream();
		}

		private void ButtonBase_OnClick4(object sender, RoutedEventArgs e)
		{
			//PatientDataBase.GenerateXmlPatientsFile(20000);
			PatientDataBase.GenerateJSONPatientsFile(20000);
		}

		private void ButtonBase_OnClick5 (object sender, RoutedEventArgs e)
		{			
			PatientDataBaseTest.GenerateJSONPatientsFile();
		}

		private void ButtonBase_OnClick6 (object sender, RoutedEventArgs e)
		{
			PatientDataBaseTest.TestLoad();
		}

		private void ButtonBase_OnClick7(object sender, RoutedEventArgs e)
		{
			var test = new MessageBusTest();

			test.SingleSubscribscriptionTest();
			test.NoSubscribscriptionTest();
		}
	}
}

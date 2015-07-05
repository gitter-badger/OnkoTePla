using System.Windows;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels;
using MahApps.Metro.Controls;


namespace bytePassion.OnkoTePla.Client.WPFVisualization
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow
	{
		public MainWindow ()
		{
			InitializeComponent();
		}

		///////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                           ///////////
		/////////                                  TestArea                                 ///////////
		/////////                                                                           ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////

		private void ButtonBase_OnClick (object sender, RoutedEventArgs e)
		{
			((AppointmentGridViewModel)((MainWindowViewModel)DataContext).AppointmentGridViewModel).TestLoad();
		}
	}
}

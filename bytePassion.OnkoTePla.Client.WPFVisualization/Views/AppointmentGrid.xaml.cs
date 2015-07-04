using System.Windows;
using System.Windows.Controls;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Views
{
	/// <summary>
	/// Interaction logic for AppointmentGrid.xaml
	/// </summary>
	public partial class AppointmentGrid : UserControl
	{
		public AppointmentGrid ()
		{
			InitializeComponent();
		}

		///////////////////////////////////////////////////////////////////////////////////////////////
		/////////                                                                           ///////////
		/////////                                  TestArea                                 ///////////
		/////////                                                                           ///////////
		///////////////////////////////////////////////////////////////////////////////////////////////

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			((AppointmentGridViewModel)DataContext).TestLoad();
		}
	}
}

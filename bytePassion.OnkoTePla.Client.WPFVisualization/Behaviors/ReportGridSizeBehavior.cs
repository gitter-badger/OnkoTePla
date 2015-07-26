using System.Windows;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class ReportGridSizeBehavior : Behavior<FrameworkElement>
	{
		
		private GlobalState<double> mainGridWidth;
		private GlobalState<double> mainGridHeight;

		private GlobalState<double> MainGridWidth  => mainGridWidth  ?? (mainGridWidth  = GetWidthVariable());
		private GlobalState<double> MainGridHeight => mainGridHeight ?? (mainGridHeight = GetHeightVariable());

		private GlobalState<double> GetHeightVariable()
		{
			return GlobalAccess.ViewModelCommunication.GetGlobalViewModelVariable<double>(GlobalVariables.AppointmentGridHeightVariable);
		}

		private GlobalState<double> GetWidthVariable()
		{
			return GlobalAccess.ViewModelCommunication.GetGlobalViewModelVariable<double>(GlobalVariables.AppointmentGridWidthVariable);
		} 

		protected override void OnAttached()
		{
			base.OnAttached();			
			AssociatedObject.SizeChanged += OnSizeChanged;
			AssociatedObject.Loaded      += OnLoaded;
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.SizeChanged -= OnSizeChanged;
			AssociatedObject.Loaded      -= OnLoaded;
		}		

		private void OnLoaded      (object sender, RoutedEventArgs e)      { ReportSize((FrameworkElement)sender); }
		private void OnSizeChanged (object sender, SizeChangedEventArgs e) { ReportSize((FrameworkElement)sender); }

		private void ReportSize(FrameworkElement sender)
		{
			MainGridWidth.Value  = sender.ActualWidth;
			MainGridHeight.Value = sender.ActualHeight;			
		}
	}
}

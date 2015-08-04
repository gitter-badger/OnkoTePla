using System.Windows;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.State;
using bytePassion.OnkoTePla.Client.WPFVisualization.GlobalAccess;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class ReportGridSizeBehavior : Behavior<FrameworkElement>
	{
				
		private GlobalState<Size> mainGridSize;
		private GlobalState<Size> MainGridSize  => mainGridSize  ?? (mainGridSize  = GetSizeVariable());		
		private GlobalState<Size> GetSizeVariable()
		{
			return Global.ViewModelCommunication.GetGlobalViewModelVariable<Size>(Global.AppointmentGridSizeVariable);
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

		private void OnLoaded      (object sender, RoutedEventArgs e)      { ReportSize((UIElement)sender); }
		private void OnSizeChanged (object sender, SizeChangedEventArgs e) { ReportSize((UIElement)sender); }

		private void ReportSize(UIElement sender)
		{
			MainGridSize.Value  = sender.RenderSize;			
		}
	}
}

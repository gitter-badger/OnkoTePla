using System.Windows;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class SizeReportingToGlobalVariebleBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty ViewModelCommunicationProperty = 
			DependencyProperty.Register(nameof(ViewModelCommunication), 
										typeof (IViewModelCommunication), 
										typeof (SizeReportingToGlobalVariebleBehavior), 
										new PropertyMetadata(default(IViewModelCommunication)));

		public IViewModelCommunication ViewModelCommunication
		{
			get { return (IViewModelCommunication) GetValue(ViewModelCommunicationProperty); }
			set { SetValue(ViewModelCommunicationProperty, value); }
		}

		private IGlobalState<Size> currentSizeVariable;
		private IGlobalState<Size> CurrentSizeVariable
		{
			get
			{
				if (ViewModelCommunication != null)
					return currentSizeVariable ?? 
						(currentSizeVariable = ViewModelCommunication.GetGlobalViewModelVariable<Size>(
													AppointmentGridSizeVariable)
												);

				return null;
			}
		} 
				
		protected override void OnAttached()
		{
			base.OnAttached();			
			AssociatedObject.SizeChanged += OnSizeChanged;	
			AssociatedObject.Loaded      += OnLoaded;
		}		

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.SizeChanged -= OnSizeChanged;
			AssociatedObject.Loaded      -= OnLoaded;
		}

		private void OnLoaded (object sender, RoutedEventArgs routedEventArgs)
		{
			ReportSize((UIElement) sender);
		}

		private void OnSizeChanged (object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			ReportSize((UIElement)sender);
		}

		private void ReportSize(UIElement sender)
		{
			if (CurrentSizeVariable != null)
				CurrentSizeVariable.Value = sender.RenderSize;			
		}
	}
}

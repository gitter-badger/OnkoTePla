using System.Windows;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class ReportSelfBehavior : Behavior<FrameworkElement>
	{
		public static readonly DependencyProperty ViewModelCommunicationProperty =
			DependencyProperty.Register(nameof(ViewModelCommunication),
										typeof (IViewModelCommunication),
										typeof (ReportSelfBehavior),
										new PropertyMetadata(default(IViewModelCommunication)));		
		
		public IViewModelCommunication ViewModelCommunication
		{
			get { return (IViewModelCommunication)GetValue(ViewModelCommunicationProperty); }
			set { SetValue(ViewModelCommunicationProperty, value); }
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.Loaded += OnLoaded;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.Loaded -= OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			ViewModelCommunication.Send(new UIInstance(AssociatedObject));
		}
	}
}

using System.Windows;
using System.Windows.Interactivity;


namespace bytePassion.Lib.WpfLib.Behaviors
{
	public class HeightWidthReportingBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty CurrentHeightProperty = 
			DependencyProperty.Register("CurrentHeight", 
										typeof (double), 
										typeof (HeightWidthReportingBehavior), 
										new PropertyMetadata(default(double)));

		public static readonly DependencyProperty CurrentWidthProperty = 
			DependencyProperty.Register("CurrentWidth", 
										typeof (double), 
										typeof (HeightWidthReportingBehavior), 
										new PropertyMetadata(default(double)));

		public double CurrentHeight
		{
			get { return (double) GetValue(CurrentHeightProperty); }
			set { SetValue(CurrentHeightProperty, value); }
		}
		
		public double CurrentWidth
		{
			get { return (double) GetValue(CurrentWidthProperty); }
			set { SetValue(CurrentWidthProperty, value); }
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
			ReportSize((FrameworkElement) sender);
		}

		private void OnSizeChanged (object sender, SizeChangedEventArgs sizeChangedEventArgs)
		{
			ReportSize((FrameworkElement)sender);
		}

		private void ReportSize(FrameworkElement sender)
		{
			CurrentHeight = sender.ActualHeight;
			CurrentWidth  = sender.ActualWidth;
		}
	}
}

using System;
using System.Windows;
using System.Windows.Interactivity;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class SizeReportingBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty CurrentWidthProperty = 
			DependencyProperty.Register("CurrentWidth",
										typeof(double),
										typeof(SizeReportingBehavior),
										new PropertyMetadata(default(Double)));

		public static readonly DependencyProperty CurrentHeightProperty = 
			DependencyProperty.Register("CurrentHeight",
										typeof(double),
										typeof(SizeReportingBehavior),
										new PropertyMetadata(default(Double)));

		public double CurrentWidth
		{
			get { return (double)GetValue(CurrentWidthProperty); }
			set { SetValue(CurrentWidthProperty, value); }
		}
		
		public double CurrentHeight
		{
			get { return (double)GetValue(CurrentHeightProperty); }
			set { SetValue(CurrentHeightProperty, value); }
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
			CurrentWidth  = sender.ActualWidth;
			CurrentHeight = sender.ActualHeight;
		}
	}
}

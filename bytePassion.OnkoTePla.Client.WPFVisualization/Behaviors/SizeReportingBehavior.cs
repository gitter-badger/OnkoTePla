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
			//AssociatedObject.LayoutUpdated += OnLayoutUpdated;
			AssociatedObject.SizeChanged   += OnSizeChanged;
		}

		void OnSizeChanged (object sender, SizeChangedEventArgs e)
		{
			CurrentWidth  = ((FrameworkElement) sender).ActualWidth;
			CurrentHeight = ((FrameworkElement) sender).ActualHeight;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			//AssociatedObject.LayoutUpdated -= OnLayoutUpdated;
			AssociatedObject.SizeChanged   -= OnSizeChanged;
		}

		private void OnLayoutUpdated(object sender, EventArgs eventArgs)
		{
			CurrentWidth = AssociatedObject.ActualWidth;
			CurrentHeight = AssociatedObject.ActualHeight;
		}
	}
}

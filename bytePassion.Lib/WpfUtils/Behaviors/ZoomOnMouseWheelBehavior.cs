using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace bytePassion.Lib.WpfUtils.Behaviors
{

	public class ZoomOnMouseWheelBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty IncreaseZoomCommandProperty = 
			DependencyProperty.Register("IncreaseZoomCommand",
										typeof(ICommand),
										typeof(ZoomOnMouseWheelBehavior));

		public static readonly DependencyProperty DecreaseZoomCommandProperty = 
			DependencyProperty.Register("DecreaseZoomCommand",
										typeof(ICommand),
										typeof(ZoomOnMouseWheelBehavior));

		public ICommand IncreaseZoomCommand
		{
			get { return (ICommand)GetValue(IncreaseZoomCommandProperty); }
			set { SetValue(IncreaseZoomCommandProperty, value);}
		}

		public ICommand DecreaseZoomCommand 
		{
			get { return (ICommand) GetValue(DecreaseZoomCommandProperty); }
			set { SetValue(DecreaseZoomCommandProperty, value);}
		}					

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseWheel += OnMouseWheel;

			Parent = (FrameworkElement)AssociatedObject.Parent;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.PreviewMouseWheel -= OnMouseWheel;
		}

		private FrameworkElement Parent { get; set; }	

		void OnMouseWheel (object sender, MouseWheelEventArgs e)
		{			
			if (e.Delta > 0)
				IncreaseZoomCommand.Execute(e.GetPosition(Parent));
			else
				DecreaseZoomCommand.Execute(e.GetPosition(Parent));
		}
	}
}

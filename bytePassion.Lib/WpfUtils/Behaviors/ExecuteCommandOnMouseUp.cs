using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace bytePassion.Lib.WpfUtils.Behaviors
{
    public class ExecuteCommandOnMouseUp : Behavior<FrameworkElement>
    {
		public static readonly DependencyProperty CommandProperty = 
			DependencyProperty.Register("Command",
									   typeof(ICommand),
									   typeof(ExecuteCommandOnMouseUp),
									   new PropertyMetadata(default(ICommand)));

		public static readonly DependencyProperty CommandParameterProperty = 
			DependencyProperty.Register("CommandParameter",
										typeof(object),
										typeof(ExecuteCommandOnMouseUp),
										new PropertyMetadata(default(object)));

		public object CommandParameter
		{
			get { return GetValue(CommandParameterProperty); }
			set { SetValue(CommandParameterProperty, value); }
		}

		public ICommand Command
		{
			get { return (ICommand)GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		protected override void OnAttached ()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
			AssociatedObject.PreviewMouseLeftButtonUp   += OnMouseLeftButtonUp;
		}

		protected override void OnDetaching ()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
			AssociatedObject.PreviewMouseLeftButtonUp   -= OnMouseLeftButtonUp;
		}

	    private bool activated = false;
	  
	    private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
	    {
		    if (activated)		    
			    if (Command != null)
				    if (Command.CanExecute(CommandParameter))
					    Command.Execute(CommandParameter);
		    
	    }	   

	    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
	    {
		    activated = true;
	    }	    
    }
}

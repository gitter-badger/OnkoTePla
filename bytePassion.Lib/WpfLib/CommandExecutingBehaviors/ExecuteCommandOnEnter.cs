using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace bytePassion.Lib.WpfLib.CommandExecutingBehaviors
{
    public class ExecuteCommandOnEnter : Behavior<FrameworkElement>
    {

	    public static readonly DependencyProperty CommandProperty = 
			DependencyProperty.Register("Command", 
										typeof (ICommand), 
										typeof (ExecuteCommandOnEnter), 
										new PropertyMetadata(default(ICommand)));

	    public static readonly DependencyProperty CommandParameterProperty = 
			DependencyProperty.Register("CommandParameter", 
										typeof (object), 
										typeof (ExecuteCommandOnEnter), 
										new PropertyMetadata(default(object)));

	    public object CommandParameter
	    {
		    get { return GetValue(CommandParameterProperty); }
		    set { SetValue(CommandParameterProperty, value); }
	    }

	    public ICommand Command
	    {
		    get { return (ICommand) GetValue(CommandProperty); }
		    set { SetValue(CommandProperty, value); }
	    }

		protected override void OnAttached ()
		{
			base.OnAttached();
			AssociatedObject.KeyDown += OnKeyDown;
		}

		protected override void OnDetaching ()
		{
			base.OnAttached();
			AssociatedObject.KeyDown -= OnKeyDown;
		}

	    private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
	    {		    
		    if (keyEventArgs.Key == Key.Enter)
				if (Command != null)
					if (Command.CanExecute(CommandParameter))
						Command.Execute(CommandParameter);
	    }	    
    }
}

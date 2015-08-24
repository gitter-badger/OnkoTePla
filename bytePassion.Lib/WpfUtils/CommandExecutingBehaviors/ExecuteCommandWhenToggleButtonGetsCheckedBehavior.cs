using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace bytePassion.Lib.WpfUtils.CommandExecutingBehaviors
{
	public class ExecuteCommandWhenToggleButtonGetsCheckedBehavior : Behavior<ToggleButton>
	{

		public static readonly DependencyProperty CommandProperty 
			= DependencyProperty.Register("Command",
										  typeof(ICommand),
										  typeof(ExecuteCommandWhenToggleButtonGetsCheckedBehavior),
										  new PropertyMetadata(null));

		public static readonly DependencyProperty CommandParameterProperty 
			= DependencyProperty.Register("CommandParameter",
										  typeof(object),
										  typeof(ExecuteCommandWhenToggleButtonGetsCheckedBehavior),
										  new PropertyMetadata(null));

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
			AssociatedObject.Checked += OnButtonChecked;			
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.Checked -= OnButtonChecked;
		}

		private void OnButtonChecked(object sender, RoutedEventArgs e)
		{
			if (Command != null)
				if (Command.CanExecute(CommandParameter))
					Command.Execute(CommandParameter);
		}

		
	}
}

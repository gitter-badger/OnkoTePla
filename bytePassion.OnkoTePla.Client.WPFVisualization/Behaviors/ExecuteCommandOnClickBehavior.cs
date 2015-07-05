using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class ExecuteCommandOnClickBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty CommandProperty 
			= DependencyProperty.Register("Command", 
										  typeof (ICommand), 
										  typeof (ExecuteCommandOnClickBehavior),
										  new PropertyMetadata(null));

		public static readonly DependencyProperty CommandParameterProperty 
			= DependencyProperty.Register("CommandParameter", 
										  typeof (object), 
										  typeof (ExecuteCommandOnClickBehavior), 
										  new PropertyMetadata(null));

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

		protected override void OnAttached()
		{
			base.OnAttached();

			AssociatedObject.MouseLeftButtonDown += OnMouseDown;
			AssociatedObject.MouseLeave          += OnMouseLeave;
			AssociatedObject.MouseLeftButtonUp   += AssociatedObjectOnMouseLeftButtonUp;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();

			AssociatedObject.MouseLeftButtonDown -= OnMouseDown;
			AssociatedObject.MouseLeave          -= OnMouseLeave;
			AssociatedObject.MouseLeftButtonUp   -= AssociatedObjectOnMouseLeftButtonUp;
		}

		private bool possibleExecution = false;

		private void AssociatedObjectOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (possibleExecution)
			{
				possibleExecution = false;
				ExecuteCommand();
			}
		}

		private void OnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
		{
			possibleExecution = false;
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			possibleExecution = true;
		}

		private void ExecuteCommand()
		{
			if (Command != null) 
				if(Command.CanExecute(CommandParameter))
					Command.Execute(CommandParameter);
		}
	}
}

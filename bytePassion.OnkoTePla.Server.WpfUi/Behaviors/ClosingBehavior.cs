using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using MahApps.Metro.Controls;

namespace bytePassion.OnkoTePla.Server.WpfUi.Behaviors
{
	internal class ClosingBehavior : Behavior<MetroWindow>
    {

	    public static readonly DependencyProperty CloseWindowCommandProperty = 
			DependencyProperty.Register(nameof(CloseWindowCommand), 
										typeof (ICommand), 
										typeof (ClosingBehavior));

	    public static readonly DependencyProperty DontCheckClosingProperty = 
			DependencyProperty.Register(nameof(CheckClosing), 
										typeof (bool), 
										typeof (ClosingBehavior));

	    public bool CheckClosing
	    {
		    get { return (bool) GetValue(DontCheckClosingProperty); }
		    set { SetValue(DontCheckClosingProperty, value); }
	    }

	    public ICommand CloseWindowCommand
	    {
		    get { return (ICommand) GetValue(CloseWindowCommandProperty); }
		    set { SetValue(CloseWindowCommandProperty, value); }
	    }

	    protected override void OnAttached()
	    {
		    base.OnAttached();

			AssociatedObject.Closing += OnClosing;
	    }	    

	    protected override void OnDetaching()
	    {
		    base.OnDetaching();

			AssociatedObject.Closing -= OnClosing;
		}

		private void OnClosing (object sender, CancelEventArgs cancelEventArgs)
		{
			if (CheckClosing)
			{
				var result = MessageBox.Show("are you really want to exit?", "Exit",
											 MessageBoxButton.YesNo,
											 MessageBoxImage.Question, 
											 MessageBoxResult.No);

				if (result == MessageBoxResult.Yes)
				{
					if (CloseWindowCommand != null)
						if (CloseWindowCommand.CanExecute(null))
							CloseWindowCommand.Execute(null);
				}

				cancelEventArgs.Cancel = true;
			}
		}
	}
}

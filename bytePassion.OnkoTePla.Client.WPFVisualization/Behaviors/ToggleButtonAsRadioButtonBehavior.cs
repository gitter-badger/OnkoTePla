using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class ToggleButtonAsRadioButtonBehavior : Behavior<ToggleButton>
	{
		public static readonly DependencyProperty SelectedOptionProperty 
			= DependencyProperty.Register("SelectedOption", 
										  typeof (RoomSelectorData), 
										  typeof (ToggleButtonAsRadioButtonBehavior), 
										  new PropertyMetadata(OnSelectionChanged));

		public static readonly DependencyProperty ButtonAssociatedOptionProperty 
			= DependencyProperty.Register("ButtonAssociatedOption", 
										  typeof (RoomSelectorData), 
										  typeof (ToggleButtonAsRadioButtonBehavior), 
										  new PropertyMetadata(default(RoomSelectorData)));

		public RoomSelectorData ButtonAssociatedOption
		{
			get { return (RoomSelectorData) GetValue(ButtonAssociatedOptionProperty); }
			set { SetValue(ButtonAssociatedOptionProperty, value); }
		}
		
		public RoomSelectorData SelectedOption
		{
			get { return (RoomSelectorData) GetValue(SelectedOptionProperty); }
			set { SetValue(SelectedOptionProperty, value); }
		}

		private static void OnSelectionChanged (DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
			var behavior = ((ToggleButtonAsRadioButtonBehavior)dependencyObject);

			SetChecked(behavior.AssociatedObject, behavior.ButtonAssociatedOption.Equals(behavior.SelectedOption));
		}

		private static void SetChecked(ToggleButton button, bool checkedValue)
		{
			if (button.IsChecked != checkedValue)
				button.IsChecked = checkedValue;
		}

		protected override void OnAttached ()
		{
			base.OnAttached();
			AssociatedObject.Checked += OnButtonChecked;			
		}		

		private void OnButtonChecked(object sender, RoutedEventArgs e)
		{
			SelectedOption = ButtonAssociatedOption;
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.Checked -= OnButtonChecked;
		}
	}
}

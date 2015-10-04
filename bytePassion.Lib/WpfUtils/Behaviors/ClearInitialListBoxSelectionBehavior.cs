using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;


namespace bytePassion.Lib.WpfUtils.Behaviors
{
	public class ClearInitialListBoxSelectionBehavior : Behavior<ListBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.Loaded += OnLoaded;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			AssociatedObject.SelectedIndex = -1;
		}
	}
}

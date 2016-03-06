using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.RoomSelector.Helper;

namespace bytePassion.OnkoTePla.Client.WpfUi.Behaviors
{
    class ResetRoomSelectionBehavior : Behavior<ListBox>
    {
        private RoomSelectorData previousSelection;

        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AddSelectionHandler;
        }

        private void AddSelectionHandler(object sender, RoutedEventArgs e)
        {
            AssociatedObject.SelectionChanged += CheckSelection;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= CheckSelection;
        }

        private void CheckSelection(object sender, SelectionChangedEventArgs e)
        {
            var vm = AssociatedObject.DataContext as RoomFilterViewModel;
            

            if (e.AddedItems.Count > 0 && e.AddedItems[0] != previousSelection)
            {
                var valid = vm?.CheckSelectionValidity((RoomSelectorData)e.AddedItems[0]);

                if (valid.Value)
                {
                    previousSelection = vm.SelectedRoomFilter;
                }

                if (valid != null && !valid.Value )
                {
                    AssociatedObject.SelectedItem = previousSelection;
                }
            }

            e.Handled = true;
        }
    }
}

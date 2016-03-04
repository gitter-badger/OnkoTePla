using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Threading;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow;
using bytePassion.OnkoTePla.Resources.UserNotificationService;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace bytePassion.OnkoTePla.Client.WpfUi.Behaviors
{
    internal class WindowClosingBehavior : Behavior<MetroWindow>
    {
        protected override void OnAttached()
        {
            AssociatedObject.Closing += Window_closing;
        }

        private async void Window_closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;

            var viewModel = AssociatedObject.DataContext as MainWindowViewModel;

            var session = viewModel.Session;

            var dialog = new UserDialogBox("Programm beenden.", "Wollen Sie das Programm wirklich beenden?",
                MessageBoxButton.OKCancel);

            var result = await dialog.ShowMahAppsDialog();


            if (result != MessageDialogResult.Affirmative)
            {
                return;
            }
            session.Logout(() =>
            {
                session.TryDisconnect(() =>
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action) (() =>
                    {
                        AssociatedObject.Closing -= Window_closing;

                        Application.Current.MainWindow.Close();
                    }));
                    e.Cancel = false;
                }, null);
            }, null);
        }
    }
}
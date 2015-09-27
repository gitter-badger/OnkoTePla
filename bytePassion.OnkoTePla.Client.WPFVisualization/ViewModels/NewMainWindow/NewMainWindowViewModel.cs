using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfUtils.Commands;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;
using MahApps.Metro;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow
{
    public class NewMainWindowViewModel : INewMainWindowViewModel
    {
        private readonly IViewModelCommunication viewModelCommunication;

        private int selectedPage;
        private bool notificationVisible;
        private INotificationViewModel notificationViewModel;

        public NewMainWindowViewModel(IOverviewPageViewModel overviewPageViewModel,
            ISearchPageViewModel searchPageViewModel,
            IOptionsPageViewModel optionsPageViewModel,
            IViewModelCommunication viewModelCommunication)
        {
            this.viewModelCommunication = viewModelCommunication;

            OverviewPageViewModel = overviewPageViewModel;
            SearchPageViewModel = searchPageViewModel;
            OptionsPageViewModel = optionsPageViewModel;

            ShowOverviewPage = new Command(() => SelectedPage = 0);
            ShowSearchPage = new Command(() => SelectedPage = 1);
            ShowOptionsPage = new Command(() => SelectedPage = 2);

            NotificationVisible = false;

           viewModelCommunication.RegisterViewModelMessageHandler<ShowNotification>(this);
            viewModelCommunication.RegisterViewModelMessageHandler<HideNotification>(this);
        }

        public int SelectedPage
        {
            get { return selectedPage; }
            private set { PropertyChanged.ChangeAndNotify(this, ref selectedPage, value); }
        }

        public ICommand ShowOverviewPage { get; }
        public ICommand ShowSearchPage { get; }
        public ICommand ShowOptionsPage { get; }

        public IOverviewPageViewModel OverviewPageViewModel { get; }
        public ISearchPageViewModel SearchPageViewModel { get; }
        public IOptionsPageViewModel OptionsPageViewModel { get; }

        public bool NotificationVisible
        {
            get { return notificationVisible; }
            private set { PropertyChanged.ChangeAndNotify(this, ref notificationVisible, value); }
        }

        public INotificationViewModel NotificationViewModel
        {
            get { return notificationViewModel; }
            private set { PropertyChanged.ChangeAndNotify(this, ref notificationViewModel, value); }
        }

        public void Process(ShowNotification message)
        {
            NotificationViewModel = new NotificationViewModel(message.NotificationMessage, viewModelCommunication);
            NotificationVisible = true;
        }

        public void Process(HideNotification message)
        {
            NotificationVisible = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
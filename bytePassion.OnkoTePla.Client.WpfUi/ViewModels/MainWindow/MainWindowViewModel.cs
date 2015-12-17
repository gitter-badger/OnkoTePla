using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.WpfUi.Factorys.ViewModelBuilder.MainViewModel;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainView;
using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.MainWindow
{
    internal class MainWindowViewModel : ViewModel, 
                                         IMainWindowViewModel
    {
        private readonly IMainViewModelBuilder mainViewModelBuilder;

        private IMainViewModel mainViewModel;
        private bool isMainViewVisible;


        public MainWindowViewModel(IMainViewModelBuilder mainViewModelBuilder)
        {
            this.mainViewModelBuilder = mainViewModelBuilder;
            MainViewModel = mainViewModelBuilder.Build();
            IsMainViewVisible = true;
        }


        public IMainViewModel MainViewModel
        {
            get { return mainViewModel; }
            private set { PropertyChanged.ChangeAndNotify(this, ref mainViewModel, value); }
        }

        public bool IsMainViewVisible
        {
            get { return isMainViewVisible; }
            private set { PropertyChanged.ChangeAndNotify(this, ref isMainViewVisible, value); }
        }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
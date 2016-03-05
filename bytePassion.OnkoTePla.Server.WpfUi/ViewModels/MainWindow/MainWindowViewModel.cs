using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.ViewModelBase;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.HoursOfOpeningPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.PatientsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
    {
		private readonly ISharedStateWriteOnly<MainPage> selectedPageVariable;

		private MainPage selectedPage;		

		public MainWindowViewModel(IOverviewPageViewModel overviewPageViewModel, 
                                   IConnectionsPageViewModel connectionsPageViewModel,
                                   IUserPageViewModel userPageViewModel,
                                   ILicencePageViewModel licencePageViewModel, 
                                   IInfrastructurePageViewModel infrastructurePageViewModel,
								   IHoursOfOpeningPageViewModel hoursOfOpeningPageViewModel,
								   ITherapyPlaceTypesPageViewModel therapyPlaceTypesPageViewModel,
								   IPatientsPageViewModel patientsPageViewModel,
								   IOptionsPageViewModel optionsPageViewModel, 
                                   IAboutPageViewModel aboutPageViewModel,
								   ISharedStateWriteOnly<MainPage> selectedPageVariable)
        {
	        this.selectedPageVariable     = selectedPageVariable;
	        PatientsPageViewModel = patientsPageViewModel;

	        OverviewPageViewModel          = overviewPageViewModel;
            ConnectionsPageViewModel       = connectionsPageViewModel;
            UserPageViewModel              = userPageViewModel;
            LicencePageViewModel           = licencePageViewModel;
            InfrastructurePageViewModel    = infrastructurePageViewModel;
	        HoursOfOpeningPageViewModel    = hoursOfOpeningPageViewModel;
            OptionsPageViewModel           = optionsPageViewModel;
            AboutPageViewModel             = aboutPageViewModel;
	        TherapyPlaceTypesPageViewModel = therapyPlaceTypesPageViewModel;

	        SwitchToPage = new ParameterrizedCommand<MainPage>(page => SelectedPage = page);
			CloseWindow = new Command(DoCloseApplication);

			CheckWindowClosing = true;
        }

		private void DoCloseApplication()
		{
			CheckWindowClosing = false;

			if (ConnectionsPageViewModel.IsConnectionActive)
			{
				ConnectionsPageViewModel.IsConnectionActive = false;				
				Application.Current.Dispatcher.DelayInvoke(KillWindow, TimeSpan.FromSeconds(3));
			}
			else
			{				
				KillWindow();
			}
		}

		private static void KillWindow()
		{
			var windows = Application.Current.Windows
											 .OfType<WpfUi.MainWindow>()
											 .ToList();
			if (windows.Count == 1)
				windows[0].Close();
			else
				throw new Exception("inner error");
		}

		public ICommand SwitchToPage { get; }

        public MainPage SelectedPage
        {
            get { return selectedPage; }
	        private set
	        {
		        PropertyChanged.ChangeAndNotify(this, ref selectedPage, value);
		        selectedPageVariable.Value = value;
	        }
        }

		public bool CheckWindowClosing { get; private set; }
		public ICommand CloseWindow { get; }
	

		public IOverviewPageViewModel          OverviewPageViewModel          { get; }
        public IConnectionsPageViewModel       ConnectionsPageViewModel       { get; }
        public IUserPageViewModel              UserPageViewModel              { get; }
        public ILicencePageViewModel           LicencePageViewModel           { get; }
        public IInfrastructurePageViewModel    InfrastructurePageViewModel    { get; }
		public IHoursOfOpeningPageViewModel    HoursOfOpeningPageViewModel    { get; }
		public ITherapyPlaceTypesPageViewModel TherapyPlaceTypesPageViewModel { get; }
		public IPatientsPageViewModel		   PatientsPageViewModel          { get; }
		public IOptionsPageViewModel           OptionsPageViewModel           { get; }
        public IAboutPageViewModel             AboutPageViewModel             { get; }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }
}

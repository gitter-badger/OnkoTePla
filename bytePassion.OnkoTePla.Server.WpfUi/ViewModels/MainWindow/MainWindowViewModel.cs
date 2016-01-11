﻿using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.OnkoTePla.Server.WpfUi.Enums;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.AboutPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.ConnectionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.LicencePage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage;
using bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage;


namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.MainWindow
{
	internal class MainWindowViewModel : ViewModel, IMainWindowViewModel
    {
		private readonly IGlobalStateWriteOnly<MainPage> selectedPageVariable;

		private MainPage selectedPage;

        public MainWindowViewModel(IOverviewPageViewModel overviewPageViewModel, 
                                   IConnectionsPageViewModel connectionsPageViewModel,
                                   IUserPageViewModel userPageViewModel,
                                   ILicencePageViewModel licencePageViewModel, 
                                   IInfrastructurePageViewModel infrastructurePageViewModel,
								   ITherapyPlaceTypesPageViewModel therapyPlaceTypesPageViewModel,
								   IOptionsPageViewModel optionsPageViewModel, 
                                   IAboutPageViewModel aboutPageViewModel,
								   IGlobalStateWriteOnly<MainPage> selectedPageVariable)
        {
	        this.selectedPageVariable     = selectedPageVariable;

	        OverviewPageViewModel          = overviewPageViewModel;
            ConnectionsPageViewModel       = connectionsPageViewModel;
            UserPageViewModel              = userPageViewModel;
            LicencePageViewModel           = licencePageViewModel;
            InfrastructurePageViewModel    = infrastructurePageViewModel;
            OptionsPageViewModel           = optionsPageViewModel;
            AboutPageViewModel             = aboutPageViewModel;
	        TherapyPlaceTypesPageViewModel = therapyPlaceTypesPageViewModel;

	        SwitchToPage = new ParameterrizedCommand<MainPage>(page => SelectedPage = page);			
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

        public IOverviewPageViewModel          OverviewPageViewModel          { get; }
        public IConnectionsPageViewModel       ConnectionsPageViewModel       { get; }
        public IUserPageViewModel              UserPageViewModel              { get; }
        public ILicencePageViewModel           LicencePageViewModel           { get; }
        public IInfrastructurePageViewModel    InfrastructurePageViewModel    { get; }
	    public ITherapyPlaceTypesPageViewModel TherapyPlaceTypesPageViewModel { get; }
	    public IOptionsPageViewModel           OptionsPageViewModel           { get; }
        public IAboutPageViewModel             AboutPageViewModel             { get; }

        protected override void CleanUp() { }
        public override event PropertyChangedEventHandler PropertyChanged;        
    }
}

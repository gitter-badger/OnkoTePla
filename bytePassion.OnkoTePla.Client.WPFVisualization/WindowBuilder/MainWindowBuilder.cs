using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.MessageBus;
using bytePassion.Lib.Communication.MessageBus.HandlerCollection;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.CommandSystem;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.Core.Readmodels;
using bytePassion.OnkoTePla.Client.Core.Repositories.Config;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessageHandler;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.ChangeConfirmationView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.GridContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NewMainWindow;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.NotificationServiceContainer;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OptionsPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.OverviewPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.RoomSelector;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.SearchPage;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.UndoRedoView;
using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.WindowBuilder
{
	public class MainWindowBuilder : IWindowBuilder<NewMainWindow>
	{
		private readonly IConfigurationReadRepository configReadRepository;
		private readonly IDataCenter dataCenter;
		private readonly ICommandBus commandBus;
		private readonly SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory;
		
		public MainWindowBuilder(IConfigurationReadRepository configReadRepository, 
								 IDataCenter dataCenter, 
								 ICommandBus commandBus,
								 SessionAndUserSpecificEventHistory sessionAndUserSpecificEventHistory)
		{
			this.configReadRepository = configReadRepository;
			this.dataCenter = dataCenter;
			this.commandBus = commandBus;
			this.sessionAndUserSpecificEventHistory = sessionAndUserSpecificEventHistory;
		}

		public NewMainWindow BuildWindow()
		{
			// initiate ViewModelCommunication			

			IHandlerCollection<ViewModelMessage> handlerCollection = new MultiHandlerCollection<ViewModelMessage>();
			IMessageBus<ViewModelMessage> viewModelMessageBus = new LocalMessageBus<ViewModelMessage>(handlerCollection);
			IStateEngine viewModelStateEngine = new StateEngine();
			IViewModelCollections viewModelCollections = new ViewModelCollections();

			IViewModelCommunication viewModelCommunication = new ViewModelCommunication(viewModelMessageBus,
																						viewModelStateEngine,
																						viewModelCollections);


			// Register Global ViewModelVariables

			var initialMedicalPractice = configReadRepository.GetAllMedicalPractices().First();  // TODO set last usage

			var gridSizeInitialValue          = new Size(400,400);
			var selectedDateInitialValue      = initialMedicalPractice.HoursOfOpening.GetLastOpenDayFromToday();
			var displayedPracticeInitialValue = initialMedicalPractice.Id;

			var adornerControl = new AdornerControl();

			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSizeVariable,              gridSizeInitialValue);
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridSelectedDateVariable,      selectedDateInitialValue);
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridDisplayedPracticeVariable, displayedPracticeInitialValue);        // TODO kann gefährlich sein ,wenn der letzte tag zu einer anderen config gehört
			viewModelCommunication.RegisterGlobalViewModelVariable(AppointmentGridRoomFilterVariable,        (Guid?)null);							// when selectedRoomID == null --> all rooms are selected
			viewModelCommunication.RegisterGlobalViewModelVariable(SideBarStateVariable,                     true);									// true --> full width; false --> minimized
			viewModelCommunication.RegisterGlobalViewModelVariable(CurrentModifiedAppointmentVariable,       (AppointmentModifications)null);		// null -> no appointment selected

			viewModelCommunication.RegisterGlobalReadOnlyViewModelVariable(AdornerControlVariable, adornerControl);

			// Create ViewModelCollection

			viewModelCommunication.CreateViewModelCollection<ITherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				TherapyPlaceRowViewModelCollection
			);

			viewModelCommunication.CreateViewModelCollection<IAppointmentGridViewModel, AggregateIdentifier>(
				AppointmentGridViewModelCollection
			);

			viewModelCommunication.CreateViewModelCollection<ITimeGridViewModel, AggregateIdentifier>(
				TimeGridViewModelCollection
			);

			viewModelCommunication.CreateViewModelCollection<IAppointmentViewModel, Guid>(
				AppointmentViewModelCollection
			);


			// register stand-alone viewModelMessageHandler

			viewModelCommunication.RegisterViewModelMessageHandler(new ConfirmChangesMessageHandler(viewModelCommunication));
			viewModelCommunication.RegisterViewModelMessageHandler(new RejectChangesMessageHandler(viewModelCommunication));

			viewModelCommunication.RegisterViewModelMessageHandler(adornerControl);

			// create permanent ViewModels

			var dateDisplayViewModel = new DateDisplayViewModel(viewModelCommunication);
			var medicalPracticeSelectorViewModel = new MedicalPracticeSelectorViewModel(dataCenter, viewModelCommunication);
			var roomSelectorViewModel = new RoomFilterViewModel(dataCenter, viewModelCommunication);
			var dateSelectorViewModel = new DateSelectorViewModel(viewModelCommunication);
			var gridContainerViewModel = new GridContainerViewModel(dataCenter,
																	commandBus,
																	viewModelCommunication,
																	new List<AggregateIdentifier>(),
																	50);
			var changeConfirmationViewModel = new ChangeConfirmationViewModel(viewModelCommunication);			
			var undoRedoViewModel = new UndoRedoViewModel(viewModelCommunication, 
														  sessionAndUserSpecificEventHistory);
			var overviewPageViewModel = new OverviewPageViewModel(dateDisplayViewModel,
																  medicalPracticeSelectorViewModel,
																  roomSelectorViewModel,
																  dateSelectorViewModel,
																  gridContainerViewModel,
																  changeConfirmationViewModel,
																  viewModelCommunication,
																  undoRedoViewModel, 
																  dataCenter);

			var searchPageViewModel   = new SearchPageViewModel();
			var optionsPageViewModel  = new OptionsPageViewModel();

			var notificationServiceContainerViewModel = new NotificationServiceContainerViewModel();

			var newMainWindowViewModel = new NewMainWindowViewModel(overviewPageViewModel,
																	searchPageViewModel,
																	optionsPageViewModel,
																	notificationServiceContainerViewModel);

			return new NewMainWindow
			{
				DataContext = newMainWindowViewModel
			};
		}

		public void DisposeWindow(NewMainWindow buildedWindow)
		{
			throw new NotImplementedException();
		}
	}
}

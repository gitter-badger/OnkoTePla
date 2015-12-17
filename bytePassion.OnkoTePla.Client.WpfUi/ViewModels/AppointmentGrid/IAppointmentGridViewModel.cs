using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Core.Domain;
using System.Collections.ObjectModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid
{
    internal interface IAppointmentGridViewModel : IViewModel,												 
												   IViewModelCollectionItem<AggregateIdentifier>,
												   IViewModelMessageHandler<Activate>,
												   IViewModelMessageHandler<Deactivate>,
												   IViewModelMessageHandler<DeleteAppointment>, 
												   IViewModelMessageHandler<SendCurrentChangesToCommandBus>,
												   IViewModelMessageHandler<CreateNewAppointmentFromModificationsAndSendToCommandBus>
	{						
		ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; } 	
		
		ITimeGridViewModel 	TimeGridViewModel { get; }

		bool PracticeIsClosedAtThisDay { get; }
		bool IsActive { get; }
	}
}

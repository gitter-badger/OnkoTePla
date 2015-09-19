using System;
using System.Collections.ObjectModel;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid
{
	public interface IAppointmentGridViewModel : IViewModel,
												 IDisposable,
												 IViewModelCollectionItem<AggregateIdentifier>,
												 IViewModelMessageHandler<Activate>,
												 IViewModelMessageHandler<Deactivate>,
												 IViewModelMessageHandler<DeleteAppointment>, 
												 IViewModelMessageHandler<SendCurrentChangesToCommandBus>
	{						
		ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; } 	
		
		ITimeGridViewModel 	TimeGridViewModel { get; }

		bool PracticeIsClosedAtThisDay { get; }
		bool IsActive { get; }
	}
}

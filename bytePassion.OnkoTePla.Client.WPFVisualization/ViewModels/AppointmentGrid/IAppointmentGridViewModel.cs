using System;
using System.Collections.ObjectModel;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid
{
	public interface IAppointmentGridViewModel : IDisposable,
												 IViewModelCollectionItem<AggregateIdentifier>,
												 IViewModelMessageHandler<Activate>,
												 IViewModelMessageHandler<Deactivate>,
												 IViewModelMessageHandler<DeleteAppointment>
	{						
		ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; } 	
		
		ITimeGridViewModel 	TimeGridViewModel { get; }
	}
}

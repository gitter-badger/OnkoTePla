using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public interface ITherapyPlaceRowViewModel : IViewModelCollectionItem<TherapyPlaceRowIdentifier>,
												 IDisposable,
												 IViewModelMessageHandler<AddAppointmentToTherapyPlaceRow>,
												 IViewModelMessageHandler<RemoveAppointmentFromTherapyPlaceRow>
	{		
		ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }				
		
		string TherapyPlaceName { get; }
		Color  RoomColor        { get; }					
	}
}
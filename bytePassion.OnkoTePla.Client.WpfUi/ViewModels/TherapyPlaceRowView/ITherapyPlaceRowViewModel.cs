using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView
{
    public interface ITherapyPlaceRowViewModel : IViewModel,
												 IViewModelCollectionItem<TherapyPlaceRowIdentifier>,
												 IViewModelCommunicationDeliverer,
												 IDataCenterDeliverer,												
												 IViewModelMessageHandler<NewSizeAvailable>,
												 IViewModelMessageHandler<AddAppointmentToTherapyPlaceRow>,
												 IViewModelMessageHandler<RemoveAppointmentFromTherapyPlaceRow>,
												 IViewModelMessageHandler<SetVisibility>
	{		
		ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }				
		
		string TherapyPlaceName { get; }
		Color  RoomColor        { get; }	
		
		Time TimeSlotBegin { get; }		
		Time TimeSlotEnd   { get; }	

		double                   GridWidth                        { get; }
		AppointmentModifications AppointmentModifications         { get; }
		AdornerControl           AdornerControl                   { get; }
		Date                     CurrentSelectedDate              { get; }
		Guid                     CurrentSelectedMedicalPracticeId { get; }
		
		bool IsVisible { get; }	
	}
}
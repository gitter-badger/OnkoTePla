using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Messages;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public class TherapyPlaceRowViewModel : ITherapyPlaceRowViewModel,
											IViewModelMessageHandler<AddAppointmentToTherapyPlaceRow>,
											IViewModelMessageHandler<RemoveAppointmentFromTherapyPlaceRow>
	{		

		public TherapyPlaceRowViewModel(TherapyPlace therapyPlace, Color roomDisplayColor,
										Time startTime, Time endTime,
										AppointmentLocalisation localisationIdentifier)
		{
			
			RoomColor = roomDisplayColor;

			TimeSlotStart = startTime;
			TimeSlotEnd   = endTime;
			LocalisationIdentifier = localisationIdentifier;

			
			TherapyPlaceName = therapyPlace.Name;

			Appointments = new ObservableCollection<IAppointmentViewModel>();			
		}

		public AppointmentLocalisation LocalisationIdentifier { get; }

		public ObservableCollection<IAppointmentViewModel> Appointments { get; }
		

		public Time   TimeSlotStart    { get; }
		public Time   TimeSlotEnd      { get; }
		public Color  RoomColor        { get; }
		
		public string TherapyPlaceName { get; }
	
		public void Process(AddAppointmentToTherapyPlaceRow message)
		{
			Appointments.Add(message.AppointmentViewModelToAdd);
		}

		public void Process(RemoveAppointmentFromTherapyPlaceRow message)
		{
			Appointments.Remove(message.AppointmentViewModelToRemove);
		}
	}
}
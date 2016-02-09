using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Core.Domain;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView
{
	internal class TherapyPlaceRowViewModelSampleData : ITherapyPlaceRowViewModel
	{
		public TherapyPlaceRowViewModelSampleData()
		{
			TherapyPlaceName = "place 1";
			RoomColor = Colors.LightBlue;						

			AppointmentViewModels = new ObservableCollection<IAppointmentViewModel>
			{
				new AppointmentViewModelSampleData( new Time(9,0), new Time(10,30)),
				new AppointmentViewModelSampleData( new Time(10,45), new Time(12,0))
			};	
			
			TimeSlotBegin = new Time( 8,0);
			TimeSlotEnd   = new Time(16,0);
			GridWidth = 600;
			IsVisible = true;

			Identifier = new TherapyPlaceRowIdentifier(new AggregateIdentifier(Date.Dummy, new Guid()), new Guid());		
		}
		
		public ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }			

		public string TherapyPlaceName { get; }
		public Color  RoomColor        { get; }
		public Time   TimeSlotBegin    { get; }
		public Time   TimeSlotEnd      { get; }
		public double GridWidth        { get; }
		public bool   IsVisible        { get; }


		public AppointmentModifications AppointmentModifications => null;
		public AdornerControl           AdornerControl           => null;		
		
		public TherapyPlaceRowIdentifier Identifier { get; }

		public void Process (NewSizeAvailable message) {}
		public void Process (AddAppointmentToTherapyPlaceRow message) {}
		public void Process (RemoveAppointmentFromTherapyPlaceRow message) {}
		public void Process (SetVisibility message) { }		

		public void Dispose () { }
		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
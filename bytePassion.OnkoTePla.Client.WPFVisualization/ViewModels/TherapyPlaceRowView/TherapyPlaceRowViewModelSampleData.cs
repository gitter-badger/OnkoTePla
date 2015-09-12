using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Requests;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView
{
	public class TherapyPlaceRowViewModelSampleData : ITherapyPlaceRowViewModel
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
			
			Identifier = new TherapyPlaceRowIdentifier(new AggregateIdentifier(Date.Dummy, new Guid()), new Guid());		
		}
		
		public ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }			

		public string TherapyPlaceName { get; }
		public Color  RoomColor        { get; }

		public TherapyPlaceRowIdentifier Identifier { get; }		

		public void   Process(AddAppointmentToTherapyPlaceRow message) {}
		public void   Process(RemoveAppointmentFromTherapyPlaceRow message) {}
		public double Process(GetMaxLeftRequest message) => -1;

		public void Dispose () {}		
	}
}
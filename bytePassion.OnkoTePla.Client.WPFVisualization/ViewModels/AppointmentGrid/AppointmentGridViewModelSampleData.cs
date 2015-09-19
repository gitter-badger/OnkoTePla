using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid
{
	public class AppointmentGridViewModelSampleData : IAppointmentGridViewModel
	{
		public AppointmentGridViewModelSampleData()
		{			
			TherapyPlaceRowViewModels = new ObservableCollection<ITherapyPlaceRowViewModel>
			{
				new TherapyPlaceRowViewModelSampleData(),
				new TherapyPlaceRowViewModelSampleData(),
				new TherapyPlaceRowViewModelSampleData(),
			};

			TimeGridViewModel = new TimeGridViewModelSampleData();	
			Identifier = new AggregateIdentifier(Date.Dummy, new Guid());
			PracticeIsClosedAtThisDay = true;
			IsActive = true;
		}
		
		public ObservableCollection<ITherapyPlaceRowViewModel> TherapyPlaceRowViewModels { get; }

		public ITimeGridViewModel TimeGridViewModel { get; }

		public bool PracticeIsClosedAtThisDay { get; }
		public bool IsActive { get; }

		public AggregateIdentifier Identifier { get; }

		public void Dispose() {}

		public void Process(Activate message) {}
		public void Process(Deactivate message) {}
		public void Process(DeleteAppointment message) {}
		public void Process(SendCurrentChangesToCommandBus message) { }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

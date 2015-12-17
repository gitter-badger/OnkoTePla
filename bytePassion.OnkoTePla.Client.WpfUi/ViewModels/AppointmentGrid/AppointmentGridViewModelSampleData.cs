using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TimeGrid;
using bytePassion.OnkoTePla.Core.Domain;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentGrid
{
    internal class AppointmentGridViewModelSampleData : IAppointmentGridViewModel
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
		public void Process (CreateNewAppointmentFromModificationsAndSendToCommandBus message) { }

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}

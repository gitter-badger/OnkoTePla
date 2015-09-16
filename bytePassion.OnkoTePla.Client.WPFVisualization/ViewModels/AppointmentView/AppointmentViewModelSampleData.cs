using System;
using System.ComponentModel;
using System.Windows.Input;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView
{
	public class AppointmentViewModelSampleData : IAppointmentViewModel
	{
		public AppointmentViewModelSampleData()
			: this(new Time(9,0), new Time(11,0))
		{			
		}

		public AppointmentViewModelSampleData(Time beginTime, Time endTime)
		{
			PatientDisplayName = "Jerry Black";
			TimeSpan = "";
			AppointmentDate = "";
			Description = "";
			Room = "";

			BeginTime = beginTime;
			EndTime = endTime;

			GridWidth = 400;
			TimeSlotBegin = new Time(8, 0);
			TimeSlotEnd = new Time(16,0);

			OperatingMode = OperatingMode.Edit;
			ShowDisabledOverlay = false;
			Identifier = new Guid();
		}

		public ICommand DeleteAppointment      => null;
		public ICommand SwitchToEditMode       => null;

		public Time   BeginTime     { get; }
		public Time   EndTime       { get; }
		public double GridWidth     { get; }
		public Time   TimeSlotBegin { get; }
		public Time   TimeSlotEnd   { get; }


		public string PatientDisplayName { get; }
		public string TimeSpan           { get; }
		public string AppointmentDate    { get; }
		public string Description        { get; }
		public string Room               { get; }				

		public OperatingMode OperatingMode  { get; set; }
		public bool ShowDisabledOverlay { get; }

//		public ICommand SaveCanvasLeftPosition => null;
//		public double   CanvasLeftPositionDelta { set {} }

		public Guid Identifier { get; }

		public void Process (Dispose message) { }
		public void Process (NewSizeAvailable message) { }

		public void Dispose() {}

		public IViewModelCommunication ViewModelCommunication { get; } = null;

		public event PropertyChangedEventHandler PropertyChanged;
		
	}
}

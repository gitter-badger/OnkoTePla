using System;
using System.Collections.ObjectModel;
using System.Windows;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.Math;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid.Helper;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid.Messages;
using Constants = bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using Duration = bytePassion.Lib.TimeLib.Duration;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TimeGrid
{
	public class TimeGridViewModel : ITimeGridViewModel, IViewModelMessageHandler<NewSizeAvailable>
	{
		
		private enum GridViewDivisionState
		{
			QuarterHours,
			HalfHours,
			Hours,
			TwoHours
		}

		private Size gridSize;

		private GridViewDivisionState gridViewDivisionState;

		private readonly Time timeSlotStart;
		private readonly Time timeSlotEnd;

		public TimeGridViewModel(AggregateIdentifier identifierWithCorrectMedPracVersion,
								 ViewModelCommunication<ViewModelMessage> viewModelCommunication,
                                 IDataCenter dataCenter,
								 Size initalSize)
		{

			viewModelCommunication.RegisterViewModelAtCollection<TimeGridViewModel, AggregateIdentifier>(
				Constants.TimeGridViewModelCollection,
				this
			);

			Identifier = identifierWithCorrectMedPracVersion;
			var medicalPractice = dataCenter.Configuration.GetMedicalPracticeByIdAndVersion(Identifier.MedicalPracticeId,
																							Identifier.PracticeVersion);
			
			timeSlotStart = medicalPractice.HoursOfOpening.GetOpeningTime(identifierWithCorrectMedPracVersion.Date);
			timeSlotEnd   = medicalPractice.HoursOfOpening.GetClosingTime(identifierWithCorrectMedPracVersion.Date);

			TimeSlotLines  = new ObservableCollection<TimeSlotLine>();
			TimeSlotLabels = new ObservableCollection<TimeSlotLabel>();

			SetnewSize(initalSize);						
		}

		public void Process (NewSizeAvailable message)
		{
			SetnewSize(message.NewSize);
		}

		private void SetnewSize(Size newGridSize)
		{
			gridSize = newGridSize;
			RecomputeGrid();
		}

		public ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; }
		public ObservableCollection<TimeSlotLine>  TimeSlotLines  { get; }

		public AggregateIdentifier Identifier { get; }

		private void RecomputeGrid ()
		{
			if (gridSize.Width < 200)
				return;

			var newGridViewDivision  = GetDevisionForWidth(gridSize.Width);
			var slotLengthInSeconds  = GetSlotLengthInSeconds(newGridViewDivision);
			var timeSlotDuration     = Time.GetDurationBetween(timeSlotEnd, timeSlotStart);
			var excactTimeSlotCount  = (double)timeSlotDuration.Seconds / slotLengthInSeconds;
			int roundedTimeSlotCount = (int)Math.Floor(excactTimeSlotCount);
			var timeSlotWidth        = gridSize.Width / excactTimeSlotCount;

			if (newGridViewDivision != gridViewDivisionState || TimeSlotLabels.Count == 0)
			{
				gridViewDivisionState = newGridViewDivision;
				CreateGridDrawing(roundedTimeSlotCount, excactTimeSlotCount,
								  slotLengthInSeconds, timeSlotWidth);
			}
			else
			{
				UpdateGridDrawing(roundedTimeSlotCount, excactTimeSlotCount, timeSlotWidth);
			}
		}

		private void CreateGridDrawing (int roundedTimeSlotCount, double excactTimeSlotCount,
										uint slotLengthInSeconds, double timeSlotWidth)
		{
			TimeSlotLabels.Clear();
			TimeSlotLines.Clear();

			for (uint slot = 0; slot < roundedTimeSlotCount + 1; slot++)
			{

				var timeCaption = new Time(timeSlotStart + new Duration(slot*slotLengthInSeconds)).ToString()
																								  .Substring(0, 5);

				TimeSlotLabels.Add(new TimeSlotLabel(timeCaption)
				{
					XCoord = slot * timeSlotWidth,
					YCoord = 30
				});

				TimeSlotLines.Add(new TimeSlotLine()
				{
					XCoord = slot * timeSlotWidth,
					YCoordTop = 60,
					YCoordBottom = gridSize.Height
				});
			}

			if (!MathLibUtils.DoubleEquals(excactTimeSlotCount, roundedTimeSlotCount))
			{
				var timeCaption = timeSlotEnd.ToString()
											 .Substring(0, 5);

				TimeSlotLabels.Add(new TimeSlotLabel(timeCaption)
				{
					XCoord = gridSize.Width,
					YCoord = 30
				});

				TimeSlotLines.Add(new TimeSlotLine()
				{
					XCoord = gridSize.Width,
					YCoordTop = 60,
					YCoordBottom = gridSize.Height
				});
			}
		}

		private void UpdateGridDrawing (int roundedTimeSlotCount, double excactTimeSlotCount, double timeSlotWidth)
		{
			for (int slot = 0; slot < roundedTimeSlotCount + 1; slot++)
			{
				var xCoord = slot*timeSlotWidth;

				TimeSlotLabels[slot].XCoord       = xCoord;
				TimeSlotLines [slot].XCoord       = xCoord;
				TimeSlotLines [slot].YCoordBottom = gridSize.Height;
			}

			if (!MathLibUtils.DoubleEquals(excactTimeSlotCount, roundedTimeSlotCount))
			{
				TimeSlotLabels[roundedTimeSlotCount + 1].XCoord       = gridSize.Width;
				TimeSlotLines [roundedTimeSlotCount + 1].XCoord       = gridSize.Width;
				TimeSlotLines [roundedTimeSlotCount + 1].YCoordBottom = gridSize.Height;
			}
		}

		private static GridViewDivisionState GetDevisionForWidth (double width)
		{ 
			if (width < Constants.ThresholdGridWidthHoursToTwoHours)         return GridViewDivisionState.TwoHours;
			if (width < Constants.ThresholdGridWidthHalfHoursToHours)        return GridViewDivisionState.Hours;
			if (width < Constants.ThresholdGridWidthQuarterHoursToHalfHours) return GridViewDivisionState.HalfHours;

			return GridViewDivisionState.QuarterHours;
		}

		private static uint GetSlotLengthInSeconds (GridViewDivisionState gridViewDivisionState)
		{
			switch (gridViewDivisionState)
			{
				case GridViewDivisionState.QuarterHours: return 900;
				case GridViewDivisionState.HalfHours:    return 1800;
				case GridViewDivisionState.Hours:        return 3600;
				case GridViewDivisionState.TwoHours:     return 7200;
			}
			throw new ArgumentException();
		}		
	}
}

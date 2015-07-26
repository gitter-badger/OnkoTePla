using System;
using System.Collections.ObjectModel;
using System.Windows;
using bytePassion.Lib.Math;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.WPFVisualization.Global;
using Duration = bytePassion.Lib.TimeLib.Duration;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGridViewModel.Helper
{
	public class GridLinesAndLabelPainting
	{
		private enum GridViewDivisionState
		{
			QuarterHours,
			HalfHours,
			Hours,
			TwoHours
		}

		private const double ThresholdGridWidthQuarterHoursToHalfHours = 1400;
		private const double ThresholdGridWidthHalfHoursToHours        = 1000;
		private const double ThresholdGridWidthHoursToTwoHours         =  600;

		private Size gridSize;		
		
		private GridViewDivisionState gridViewDivisionState;

		private Time timeSlotStart;
		private Time timeSlotEnd;

		public GridLinesAndLabelPainting ()
		{			
			TimeSlotLabels = new ObservableCollection<TimeSlotLabel>();
			TimeSlotLines  = new ObservableCollection<TimeSlotLine>();

			SetNewTimeSpan(new Time(7, 0), new Time(16, 0));

			var globalGridSizeVariable = GlobalAccess.ViewModelCommunication.GetGlobalViewModelVariable<Size>(GlobalVariables.AppointmentGridSizeVariable);
			gridSize = globalGridSizeVariable.Value;
			globalGridSizeVariable.StateChanged += OnGridSizeChanged;
		}

		private void OnGridSizeChanged(Size newGridSize)
		{
			gridSize = newGridSize;
			RecomputeGrid(false);
		}
		
		public ObservableCollection<TimeSlotLabel> TimeSlotLabels { get; }
		public ObservableCollection<TimeSlotLine>  TimeSlotLines  { get; }

		public void SetNewTimeSpan (Time newStartTime, Time newEndTime)
		{
			timeSlotStart = newStartTime;
			timeSlotEnd   = newEndTime;

			RecomputeGrid(true);
		}		

		private void RecomputeGrid (bool forceCreation)
		{
			if (gridSize.Width < 200)
				return;

			var newGridViewDivision  = GetDevisionForWidth(gridSize.Width);
			var slotLengthInSeconds  = GetSlotLengthInSeconds(newGridViewDivision);
			var timeSlotDuration     = Time.GetDurationBetween(timeSlotEnd, timeSlotStart);
			var excactTimeSlotCount  = (double)timeSlotDuration.Seconds / slotLengthInSeconds;
			int roundedTimeSlotCount = (int)Math.Floor(excactTimeSlotCount);
			var timeSlotWidth        = gridSize.Width / excactTimeSlotCount;

			if (forceCreation || newGridViewDivision != gridViewDivisionState || TimeSlotLabels.Count == 0)
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

				TimeSlotLabels[slot].XCoord = xCoord;
				TimeSlotLines[slot].XCoord  = xCoord;
				TimeSlotLines[slot].YCoordBottom = gridSize.Height;
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
			if (width < ThresholdGridWidthHoursToTwoHours)         return GridViewDivisionState.TwoHours;
			if (width < ThresholdGridWidthHalfHoursToHours)        return GridViewDivisionState.Hours;
			if (width < ThresholdGridWidthQuarterHoursToHalfHours) return GridViewDivisionState.HalfHours;

			return GridViewDivisionState.QuarterHours;
		}

		private static uint GetSlotLengthInSeconds (GridViewDivisionState gridViewDivisionState)
		{
			switch (gridViewDivisionState)
			{
				case GridViewDivisionState.QuarterHours: return  900;
				case GridViewDivisionState.HalfHours:    return 1800;
				case GridViewDivisionState.Hours:        return 3600;
				case GridViewDivisionState.TwoHours:     return 7200;
			}
			throw new ArgumentException();
		}
	}
}

using System;
using System.Collections.ObjectModel;
using bytePassion.Lib.Math;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper
{
	public class GridLinesAndLabelPainting
	{
		private enum GridViewDivision
		{
			QuarterHours,
			HalfHours,
			Hours,
			TwoHours
		}

		private const double ThresholdGridWidthQuarterHoursToHalfHours = 1400;
		private const double ThresholdGridWidthHalfHoursToHours        = 1000;
		private const double ThresholdGridWidthHoursToTwoHours         =  600;

		private double gridWidth;
		private double gridHeight;

		private GridViewDivision gridViewDivision;

		private Time timeSlotStart;
		private Time timeSlotEnd;

		private readonly ObservableCollection<TimeSlotLabel> timeSlotLabels;
		private readonly ObservableCollection<TimeSlotLine>  timeSlotLines;

		public GridLinesAndLabelPainting ()
		{			
			timeSlotLabels = new ObservableCollection<TimeSlotLabel>();
			timeSlotLines  = new ObservableCollection<TimeSlotLine>();

			SetNewTimeSpan(new Time(7, 0), new Time(16, 0));
		}

		public ObservableCollection<TimeSlotLabel> TimeSlotLabels { get { return timeSlotLabels; }}
		public ObservableCollection<TimeSlotLine>  TimeSlotLines  { get { return timeSlotLines;  }}

		public void SetNewTimeSpan (Time newStartTime, Time newEndTime)
		{
			timeSlotStart = newStartTime;
			timeSlotEnd   = newEndTime;

			RecomputeGrid(true);
		}

		public void SetNewGridWidth (double newGridWidth)
		{
			gridWidth = newGridWidth;
			RecomputeGrid(false);
		}

		public void SetNewGridHeight (double newGridHeight)
		{
			gridHeight = newGridHeight;
			RecomputeGrid(false);
		}

		private void RecomputeGrid (bool forceCreation)
		{
			if (gridWidth < 200)
				return;

			var newGridViewDivision  = GetDevisionForWidth(gridWidth);
			var slotLengthInSeconds  = GetSlotLengthInSeconds(newGridViewDivision);
			var timeSlotDuration     = Time.GetDurationBetween(timeSlotEnd, timeSlotStart);
			var excactTimeSlotCount  = (double)timeSlotDuration.Seconds / slotLengthInSeconds;
			int roundedTimeSlotCount = (int)Math.Floor(excactTimeSlotCount);
			var timeSlotWidth        = gridWidth / excactTimeSlotCount;

			if (forceCreation || newGridViewDivision != gridViewDivision || TimeSlotLabels.Count == 0)
			{
				gridViewDivision = newGridViewDivision;
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
			timeSlotLabels.Clear();
			timeSlotLines.Clear();

			for (uint slot = 0; slot < roundedTimeSlotCount + 1; slot++)
			{

				var timeCaption = new Time(timeSlotStart + new Duration(slot*slotLengthInSeconds)).ToString()
																							      .Substring(0, 5);

				timeSlotLabels.Add(new TimeSlotLabel(timeCaption)
				{
					XCoord = slot * timeSlotWidth,
					YCoord = 30
				});

				timeSlotLines.Add(new TimeSlotLine()
				{
					XCoord = slot * timeSlotWidth,
					YCoordTop = 60,
					YCoordBottom = gridHeight
				});
			}

			if (!MathLibExtension.DoubleEquals(excactTimeSlotCount, roundedTimeSlotCount))
			{
				var timeCaption = timeSlotEnd.ToString()
											 .Substring(0, 5);

				timeSlotLabels.Add(new TimeSlotLabel(timeCaption)
				{
					XCoord = gridWidth,
					YCoord = 30
				});

				timeSlotLines.Add(new TimeSlotLine()
				{
					XCoord = gridWidth,
					YCoordTop = 60,
					YCoordBottom = gridHeight
				});
			}
		}

		private void UpdateGridDrawing (int roundedTimeSlotCount, double excactTimeSlotCount, double timeSlotWidth)
		{ 
			for (int slot = 0; slot < roundedTimeSlotCount + 1; slot++)
			{
				var xCoord = slot*timeSlotWidth;

				timeSlotLabels[slot].XCoord = xCoord;
				timeSlotLines[slot].XCoord  = xCoord;
				timeSlotLines[slot].YCoordBottom = gridHeight;
			}

			if (!MathLibExtension.DoubleEquals(excactTimeSlotCount, roundedTimeSlotCount))
			{
				timeSlotLabels[roundedTimeSlotCount + 1].XCoord       = gridWidth;
				timeSlotLines [roundedTimeSlotCount + 1].XCoord       = gridWidth;
				timeSlotLines [roundedTimeSlotCount + 1].YCoordBottom = gridHeight;
			}
		}

		private static GridViewDivision GetDevisionForWidth (double width)
		{
			if (width < ThresholdGridWidthHoursToTwoHours)         return GridViewDivision.TwoHours;
			if (width < ThresholdGridWidthHalfHoursToHours)        return GridViewDivision.Hours;
			if (width < ThresholdGridWidthQuarterHoursToHalfHours) return GridViewDivision.HalfHours;

			return GridViewDivision.QuarterHours;
		}

		private static uint GetSlotLengthInSeconds (GridViewDivision gridViewDivision)
		{
			switch (gridViewDivision)
			{
				case GridViewDivision.QuarterHours: return  900;
				case GridViewDivision.HalfHours:    return 1800;
				case GridViewDivision.Hours:        return 3600;
				case GridViewDivision.TwoHours:     return 7200;
			}
			throw new ArgumentException();
		}
	}
}

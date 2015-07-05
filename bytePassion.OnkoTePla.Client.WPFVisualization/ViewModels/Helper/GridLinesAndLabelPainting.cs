using System;
using System.Collections.ObjectModel;
using bytePassion.Lib.Math;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper
{
	public class GridLinesAndLabelPainting
	{
		private enum GridViewDivision { QuarterHours, HalfHours, Hours, TwoHours }

		private const double ThresholdQuarterHoursToHalfHours = 1400;
		private const double ThresholdHalfHoursToHours        = 1000;
		private const double ThresholdHoursToTwoHours         =  600;

		private double currentGridWidth;
		private double currentGridHeight;

		private GridViewDivision currentGridViewDivision;

		private Time startTime;
		private Time endTime;

		private readonly ObservableCollection<TimeSlotLabel> timeSlotLabels;
		private readonly ObservableCollection<TimeSlotLine>  timeSlotLines;

		public GridLinesAndLabelPainting()
		{
			startTime = new Time(7, 0);
			endTime   = new Time(16, 0);

			timeSlotLabels = new ObservableCollection<TimeSlotLabel>();
			timeSlotLines  = new ObservableCollection<TimeSlotLine>();

			CreateGridDrawing(GridViewDivision.TwoHours);
		}

		public ObservableCollection<TimeSlotLabel> TimeSlotLabels { get { return timeSlotLabels; }}
		public ObservableCollection<TimeSlotLine>  TimeSlotLines  { get { return timeSlotLines;  }}

		public void SetNewTimeSpan(Time newStartTime, Time newEndTime)
		{
			startTime = newStartTime;
			endTime = newEndTime;

			CreateGridDrawing(GetDevisionForWidth(currentGridWidth));
		}

		public void SetNewGridWidth(double gridWidth)
		{
			currentGridWidth = gridWidth;
			RecomputeGrid();
		}

		public void SetNewGridHeight(double gridHeight)
		{
			currentGridHeight = gridHeight;
			RecomputeGrid();
		}

		private void RecomputeGrid ()
		{
			if (currentGridWidth < 200)
				return;

			var gridViewDivision    = GetDevisionForWidth(currentGridWidth);

			if (gridViewDivision == currentGridViewDivision)
				UpdateGridDrawing(gridViewDivision);
			else
				CreateGridDrawing(gridViewDivision);
		}

		private void CreateGridDrawing (GridViewDivision gridViewDivision)
		{
			currentGridViewDivision = gridViewDivision;

			var duration = Time.GetDurationBetween(endTime, startTime);

			var slotLengthInSeconds = GetSlotLengthInSeconds(gridViewDivision);

			double excactTimeSlotCount = (double)duration.Seconds / slotLengthInSeconds;
			int roundedTimeSlotCount = (int)Math.Floor(excactTimeSlotCount);

			var timeSlotWidth = currentGridWidth / excactTimeSlotCount;

			timeSlotLabels.Clear();
			timeSlotLines.Clear();

			for (uint slot = 0; slot < roundedTimeSlotCount + 1; slot++)
			{

				var timeCaption = new Time(startTime + new Duration(slot*slotLengthInSeconds)).ToString()
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
					YCoordBottom = currentGridHeight
				});
			}

			if (!MathLibExtension.DoubleEquals(excactTimeSlotCount, roundedTimeSlotCount))
			{
				var timeCaption = endTime.ToString().Substring(0, 5);

				timeSlotLabels.Add(new TimeSlotLabel(timeCaption)
				{
					XCoord = currentGridWidth,
					YCoord = 30
				});

				timeSlotLines.Add(new TimeSlotLine()
				{
					XCoord = currentGridWidth,
					YCoordTop = 60,
					YCoordBottom = currentGridHeight
				});
			}
		}

		private void UpdateGridDrawing (GridViewDivision gridViewDivision)
		{
			var duration            = Time.GetDurationBetween(endTime, startTime);
			var slotLengthInSeconds = GetSlotLengthInSeconds(gridViewDivision);

			double excactTimeSlotCount = (double)duration.Seconds / slotLengthInSeconds;
			int roundedTimeSlotCount   = (int)Math.Floor(excactTimeSlotCount);

			var timeSlotWidth = currentGridWidth / excactTimeSlotCount;

			for (int slot = 0; slot < roundedTimeSlotCount + 1; slot++)
			{
				var xCoord = slot*timeSlotWidth;

				timeSlotLabels[slot].XCoord = xCoord;
				timeSlotLines[slot].XCoord  = xCoord;
				timeSlotLines[slot].YCoordBottom = currentGridHeight;
			}

			if (!MathLibExtension.DoubleEquals(excactTimeSlotCount, roundedTimeSlotCount))
			{
				timeSlotLabels[roundedTimeSlotCount + 1].XCoord      = currentGridWidth;
				timeSlotLines[roundedTimeSlotCount + 1].XCoord       = currentGridWidth;
				timeSlotLines[roundedTimeSlotCount + 1].YCoordBottom = currentGridHeight;
			}
		}

		private GridViewDivision GetDevisionForWidth (double width)
		{
			if (width < ThresholdHoursToTwoHours)         return GridViewDivision.TwoHours;
			if (width < ThresholdHalfHoursToHours)        return GridViewDivision.Hours;
			if (width < ThresholdQuarterHoursToHalfHours) return GridViewDivision.HalfHours;

			return GridViewDivision.QuarterHours;
		}

		private uint GetSlotLengthInSeconds (GridViewDivision gridViewDivision)
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

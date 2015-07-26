using System.ComponentModel;
using bytePassion.Lib.FrameworkExtensions;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentGrid.Helper
{
	public class TimeSlotLabel : INotifyPropertyChanged
	{
		private double xCoord;
		private double yCoord;

		private readonly string caption;

		public TimeSlotLabel(string caption)
		{
			this.caption = caption;
		}

		public double XCoord
		{
			get { return xCoord; }
			set { PropertyChanged.ChangeAndNotify(this, ref xCoord, value); }
		}

		public double YCoord
		{
			get { return yCoord; }
			set { PropertyChanged.ChangeAndNotify(this, ref yCoord, value); }
		}

		public string Caption
		{
			get { return caption; }
		}

		public event PropertyChangedEventHandler PropertyChanged;		
	}
}
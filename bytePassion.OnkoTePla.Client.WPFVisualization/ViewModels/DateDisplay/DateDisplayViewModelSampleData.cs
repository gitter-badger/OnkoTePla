using System.ComponentModel;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.DateDisplay
{
	public class DateDisplayViewModelSampleData : IDateDisplayViewModel
    {
	    public DateDisplayViewModelSampleData()
	    {
		    Date = "30.05.2015";
	    }

	    public string Date { get; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

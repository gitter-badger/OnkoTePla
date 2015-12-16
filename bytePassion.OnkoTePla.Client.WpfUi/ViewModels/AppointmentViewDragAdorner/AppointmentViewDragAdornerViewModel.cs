using bytePassion.Lib.FrameworkExtensions;
using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentViewDragAdorner
{
    public class AppointmentViewDragAdornerViewModel : ViewModel, 
                                                       IAppointmentViewDragAdornerViewModel
	{
		private bool dropPossible;

		public AppointmentViewDragAdornerViewModel(string content)
		{
			Content = content;
			DropPossible = false;
		}

		public bool DropPossible
		{
			get { return dropPossible; }
			set { PropertyChanged.ChangeAndNotify(this, ref dropPossible, value); }
		}

		public string Content { get; }
		
	    protected override void CleanUp() {	 }
        public override event PropertyChangedEventHandler PropertyChanged;
    }
}

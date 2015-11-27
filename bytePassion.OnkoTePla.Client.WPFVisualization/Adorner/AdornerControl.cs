using bytePassion.Lib.WpfLib.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewDragAdorner;
using System.Windows;
using System.Windows.Documents;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Adorner
{
    public class AdornerControl 
	{		
		
		private UIElementAdorner appointmentAdorner;
		private IAppointmentViewDragAdornerViewModel adornerViewModel;

		private double currentAdornerWidth;

		public void CreateAdorner(string content, double width)
		{
			if (appointmentAdorner == null && ReferenceElement != null)
			{
				currentAdornerWidth = width;
				adornerViewModel = new AppointmentViewDragAdornerViewModel(content);

				var adornerView = new Views.AppointmentViewDragAdorner
				                  {
					                  DataContext = adornerViewModel,
					                  Height = 30,
					                  Width = width
				                  };

				appointmentAdorner = new UIElementAdorner(ReferenceElement,
				                                          adornerView,
				                                          AdornerLayer.GetAdornerLayer(ReferenceElement));
			}
		}

	    public UIElement ReferenceElement { get; set; }
	
		public void NewMousePositionForAdorner (Point newMousePosition)
		{
			appointmentAdorner.UpdatePosition(newMousePosition.X-(currentAdornerWidth / 2), newMousePosition.Y - 15);
		}

		public void ShowAdornerLikeDropIsPossible ()
		{
			adornerViewModel.DropPossible = true;
		}

		public void ShowAdornerLikeDropIsNotPossible ()
		{
			adornerViewModel.DropPossible = false;
		}

		public void DisposeAdorner ()
		{
			appointmentAdorner?.Destroy();
			appointmentAdorner = null;
		}         		
	}
}

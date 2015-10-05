using System.Windows;
using System.Windows.Documents;
using bytePassion.Lib.Communication.ViewModel.Messages;
using bytePassion.Lib.WpfUtils.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentViewDragAdorner;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Adorner
{
	public class AdornerControl : IViewModelMessageHandler<UIInstance>
	{		
		private UIElement appointmentGridContainer;

		private UIElementAdorner appointmentAdorner;
		private IAppointmentViewDragAdornerViewModel adornerViewModel;

		private double currentAdornerWidth;

		public void CreateAdorner(string content, double width)
		{
			if (appointmentAdorner == null && appointmentGridContainer != null)
			{
				currentAdornerWidth = width;
				adornerViewModel = new AppointmentViewDragAdornerViewModel(content);

				var adornerView = new Views.AppointmentViewDragAdorner
				                  {
					                  DataContext = adornerViewModel,
					                  Height = 30,
					                  Width = width
				                  };

				appointmentAdorner = new UIElementAdorner(appointmentGridContainer,
				                                          adornerView,
				                                          AdornerLayer.GetAdornerLayer(appointmentGridContainer));
			}
		}

		public UIElement ReferenceElement { get { return appointmentGridContainer; }}

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

		public void Process(UIInstance message)
		{
			appointmentGridContainer = message.Instance;
		}
	}
}

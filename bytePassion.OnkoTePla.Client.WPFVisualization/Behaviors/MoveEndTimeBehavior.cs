using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class MoveEndTimeBehavior : Behavior<FrameworkElement>
	{
				
		public static readonly DependencyProperty ViewModelCommunicationProperty =
			DependencyProperty.Register(nameof(ViewModelCommunication), 
										typeof (IViewModelCommunication), 
										typeof (MoveEndTimeBehavior), 
										new PropertyMetadata(default(IViewModelCommunication)));
		 
		public IViewModelCommunication ViewModelCommunication
		{
			get { return (IViewModelCommunication) GetValue(ViewModelCommunicationProperty); }
			set { SetValue(ViewModelCommunicationProperty, value); }
		}		

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseLeftButtonDown += OnAssociatedObjectMouseLeftButtonDown;
			AssociatedObject.MouseLeftButtonUp          += OnAssociatedObjectMouseLeftButtonUp;
			AssociatedObject.MouseMove                  += OnAssociatedObjectMouseMove;

			container = Application.Current.MainWindow;
			mouseIsDown = false;
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.PreviewMouseLeftButtonDown -= OnAssociatedObjectMouseLeftButtonDown;
			AssociatedObject.MouseLeftButtonUp          -= OnAssociatedObjectMouseLeftButtonUp;
			AssociatedObject.MouseMove                  -= OnAssociatedObjectMouseMove;
		}

		private FrameworkElement container;
		
		private bool  mouseIsDown;		
		private Point referencePoint;

		private IGlobalState<AppointmentModifications> currentModifiedAppointmentVariable; 

		private void OnAssociatedObjectMouseMove(object sender, MouseEventArgs mouseEventArgs)
		{
			if (mouseIsDown)
			{
				var displacement = mouseEventArgs.GetPosition(container) - referencePoint;

				currentModifiedAppointmentVariable.Value.SetNewEndTimeDelta(displacement.X);				
			}
		}

		private void OnAssociatedObjectMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			EndDrag();
		}

		private void OnAssociatedObjectMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{			
			InitDrag(mouseButtonEventArgs.GetPosition(container));

			mouseButtonEventArgs.Handled = true;
		}

		private void InitDrag (Point startinPoint)
		{
			mouseIsDown = true;
			referencePoint = startinPoint;

			currentModifiedAppointmentVariable = ViewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable
			);

			AssociatedObject.CaptureMouse();
		}

		private void EndDrag ()
		{
			currentModifiedAppointmentVariable.Value.FixEndTimeDelta();
			mouseIsDown = false;
			Mouse.Capture(null);
		}
	}
}

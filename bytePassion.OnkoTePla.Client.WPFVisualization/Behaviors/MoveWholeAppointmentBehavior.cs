using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.Adorner;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.Behaviors
{
	public class MoveWholeAppointmentBehavior : Behavior<FrameworkElement>
	{

		public static readonly DependencyProperty ViewModelCommunicationProperty =
			DependencyProperty.Register(nameof(ViewModelCommunication),
										typeof (IViewModelCommunication),
										typeof (MoveWholeAppointmentBehavior),
										new PropertyMetadata(default(IViewModelCommunication)));

		public IViewModelCommunication ViewModelCommunication
		{
			get { return (IViewModelCommunication)GetValue(ViewModelCommunicationProperty); }
			set { SetValue(ViewModelCommunicationProperty, value); }
		}

		protected override void OnAttached ()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseLeftButtonDown += OnAssociatedObjectMouseLeftButtonDown;
			AssociatedObject.MouseLeftButtonUp          += OnAssociatedObjectMouseLeftButtonUp;
			AssociatedObject.MouseMove                  += OnAssociatedObjectMouseMove;
			AssociatedObject.MouseLeave                 += AssociatedObjectOnMouseLeave;
			AssociatedObject.PreviewQueryContinueDrag   += OnQueryContinueDrag;
			AssociatedObject.GiveFeedback               += OnGiveFeedback;

			container = Application.Current.MainWindow;
			mouseIsDown = false;
		}

		protected override void OnDetaching ()
		{
			base.OnDetaching();
			AssociatedObject.PreviewMouseLeftButtonDown -= OnAssociatedObjectMouseLeftButtonDown;
			AssociatedObject.MouseLeftButtonUp          -= OnAssociatedObjectMouseLeftButtonUp;
			AssociatedObject.MouseMove                  -= OnAssociatedObjectMouseMove;
			AssociatedObject.MouseLeave                 -= AssociatedObjectOnMouseLeave;
			AssociatedObject.PreviewQueryContinueDrag   -= OnQueryContinueDrag;
			AssociatedObject.GiveFeedback               -= OnGiveFeedback;
		}

		private void AssociatedObjectOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
		{
			if (mouseIsDown)
			{
				EndDrag();				

				ViewModelCommunication.SendTo(
					AppointmentViewModelCollection,
					currentModifiedAppointmentVariable.Value.OriginalAppointment.Id, 
					new ShowDisabledOverlay()
				);

				var adornerControl = ViewModelCommunication.GetGlobalViewModelVariable<AdornerControl>(
					AdornerControlVariable
				).Value;

				adornerControl.CreateAdorner(currentModifiedAppointmentVariable.Value.OriginalAppointment.Patient.Name,
											 currentModifiedAppointmentVariable.Value.CurrentAppointmentPixelWidth);
				

				DragDrop.DoDragDrop((DependencyObject)sender,
									currentModifiedAppointmentVariable.Value.OriginalAppointment,
				                    DragDropEffects.Link);

				adornerControl.DisposeAdorner();

				ViewModelCommunication.SendTo(
					AppointmentViewModelCollection,
					currentModifiedAppointmentVariable.Value.OriginalAppointment.Id,
					new HideDisabledOverlay()
				);
			}
		}

		private void OnQueryContinueDrag (object sender, QueryContinueDragEventArgs e)
		{
			if (e.EscapePressed)
			{
				e.Action = DragAction.Cancel;
				e.Handled = true;
			}			
		}		

		private void OnGiveFeedback(object sender, GiveFeedbackEventArgs giveFeedbackEventArgs)
		{
			Mouse.SetCursor(Cursors.None);
			giveFeedbackEventArgs.UseDefaultCursors = false;
			giveFeedbackEventArgs.Handled = true;
		}

		private FrameworkElement container;

		private bool  mouseIsDown;
		private Point referencePoint;

		private IGlobalState<AppointmentModifications> currentModifiedAppointmentVariable;

	    private void OnAssociatedObjectMouseMove (object sender, MouseEventArgs mouseEventArgs)
		{
			if (mouseIsDown)
			{
			    var mousePos = mouseEventArgs.GetPosition(container);
                var displacement =  mousePos- referencePoint;
				currentModifiedAppointmentVariable.Value.SetNewTimeShiftDelta(displacement.X);
			}
		}

		private void OnAssociatedObjectMouseLeftButtonUp (object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			if (mouseIsDown)
				EndDrag();
		}

		private void OnAssociatedObjectMouseLeftButtonDown (object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			InitDrag(mouseButtonEventArgs.GetPosition(container));

			mouseButtonEventArgs.Handled = true;
		}

		private void InitDrag (Point startinPoint)
		{
			

			currentModifiedAppointmentVariable = ViewModelCommunication.GetGlobalViewModelVariable<AppointmentModifications>(
				CurrentModifiedAppointmentVariable
			);

			if (currentModifiedAppointmentVariable.Value != null)
			{

				mouseIsDown = true;
				referencePoint = startinPoint;

				//AssociatedObject.CaptureMouse();
			}
		}

		private void EndDrag ()
		{
			currentModifiedAppointmentVariable.Value.FixTimeShiftDelta();
			mouseIsDown = false;
		    //Mouse.Capture(null);
		}
	}
}

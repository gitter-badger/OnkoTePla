using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.OnkoTePla.Client.DataAndService.Model;
using bytePassion.OnkoTePla.Client.WpfUi.Adorner;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AppointmentView.Helper;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;


namespace bytePassion.OnkoTePla.Client.WpfUi.ViewModels.TherapyPlaceRowView
{
    internal class TherapyPlaceRowViewModel : ViewModel,
											  ITherapyPlaceRowViewModel
	{
		private readonly IGlobalState<AppointmentModifications> appointmentModificationsVariable;
		private readonly IGlobalState<Date> selectedDateVariable;
		private readonly IGlobalState<Guid> selectedMedicalPracticeIdVariable;

		private double gridWidth;
		private bool isVisible;
		private AppointmentModifications appointmentModifications;
		private Date currentSelectedDate;
		private Guid currentSelectedMedicalPracticeId;

		public TherapyPlaceRowViewModel(IViewModelCommunication viewModelCommunication, 
									    IDataCenter dataCenter,
										TherapyPlace therapyPlace, 
										Color roomDisplayColor,										
										TherapyPlaceRowIdentifier identifier,
										AdornerControl adornerControl,
										IGlobalState<AppointmentModifications> appointmentModificationsVariable, 
										IGlobalState<Date> selectedDateVariable, 
										IGlobalState<Guid> selectedMedicalPracticeIdVariable)
		{
			this.appointmentModificationsVariable = appointmentModificationsVariable;
			this.selectedDateVariable = selectedDateVariable;
			this.selectedMedicalPracticeIdVariable = selectedMedicalPracticeIdVariable;

			appointmentModificationsVariable.StateChanged += OnAppointmentModificationsChanged;
			selectedDateVariable.StateChanged += OnSelectedDateChanged;
			selectedMedicalPracticeIdVariable.StateChanged += OnSelectedMedicalPracticeIdChanged;

			OnSelectedDateChanged(selectedDateVariable.Value);
			OnSelectedMedicalPracticeIdChanged(selectedMedicalPracticeIdVariable.Value);
			OnAppointmentModificationsChanged(appointmentModificationsVariable.Value);

			ViewModelCommunication = viewModelCommunication;
			DataCenter = dataCenter;

			IsVisible		 = true;
			RoomColor        = roomDisplayColor;		
			Identifier       = identifier;			
			TherapyPlaceName = therapyPlace.Name;

			AppointmentViewModels = new ObservableCollection<IAppointmentViewModel>();	
			
			viewModelCommunication.RegisterViewModelAtCollection<ITherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				Constants.TherapyPlaceRowViewModelCollection,
				this	
			);

			var medicalPractice = dataCenter.GetMedicalPracticeByIdAndDate(identifier.PlaceAndDate.MedicalPracticeId,
                                                                           identifier.PlaceAndDate.Date);

			TimeSlotBegin = medicalPractice.HoursOfOpening.GetOpeningTime(identifier.PlaceAndDate.Date);
			TimeSlotEnd   = medicalPractice.HoursOfOpening.GetClosingTime(identifier.PlaceAndDate.Date);

			AdornerControl = adornerControl;			
		}

		private void OnSelectedMedicalPracticeIdChanged(Guid newMedicalPracticeId)
		{
			CurrentSelectedMedicalPracticeId = newMedicalPracticeId;
		}

		private void OnSelectedDateChanged(Date newDate)
		{
			CurrentSelectedDate = newDate;
		}

		private void OnAppointmentModificationsChanged(AppointmentModifications newAppointmentModifications)
		{
			AppointmentModifications = newAppointmentModifications;
		}

		public TherapyPlaceRowIdentifier Identifier { get; }

		public ObservableCollection<IAppointmentViewModel> AppointmentViewModels { get; }		
		
		public Color  RoomColor        { get; }		
		public string TherapyPlaceName { get; }

		public Time TimeSlotBegin { get; }
		public Time TimeSlotEnd   { get; }

		public double GridWidth
		{
			get { return gridWidth; }
			private set { PropertyChanged.ChangeAndNotify(this, ref gridWidth, value); }
		}

		public AdornerControl AdornerControl { get; }

		public AppointmentModifications AppointmentModifications
		{
			get { return appointmentModifications; }
			private set { PropertyChanged.ChangeAndNotify(this, ref appointmentModifications, value); }
		}

		public Date CurrentSelectedDate
		{
			get { return currentSelectedDate; }
			private set { PropertyChanged.ChangeAndNotify(this, ref currentSelectedDate, value); }
		}

		public Guid CurrentSelectedMedicalPracticeId
		{
			get { return currentSelectedMedicalPracticeId; }
			private set { PropertyChanged.ChangeAndNotify(this, ref currentSelectedMedicalPracticeId, value); }
		}

		public bool IsVisible
		{
			get { return isVisible; }
			private set { PropertyChanged.ChangeAndNotify(this, ref isVisible, value); }
		}

		public void Process (NewSizeAvailable message)
		{
			GridWidth = message.NewSize.Width;
		}

		public void Process(AddAppointmentToTherapyPlaceRow message)
		{
			AppointmentViewModels.Add(message.AppointmentViewModelToAdd);
		}

		public void Process(RemoveAppointmentFromTherapyPlaceRow message)
		{
			AppointmentViewModels.Remove(message.AppointmentViewModelToRemove);
		}

		public void Process (SetVisibility message)
		{
			IsVisible = message.Visible;
		}

        protected override void CleanUp()
		{
			ViewModelCommunication.DeregisterViewModelAtCollection<TherapyPlaceRowViewModel, TherapyPlaceRowIdentifier>(
				Constants.TherapyPlaceRowViewModelCollection,
				this
			);

			appointmentModificationsVariable.StateChanged  -= OnAppointmentModificationsChanged;
			selectedDateVariable.StateChanged              -= OnSelectedDateChanged;
			selectedMedicalPracticeIdVariable.StateChanged -= OnSelectedMedicalPracticeIdChanged;
		}

		public IViewModelCommunication ViewModelCommunication { get; }
		public IDataCenter DataCenter { get; }

		public override event PropertyChangedEventHandler PropertyChanged;		
	}
}
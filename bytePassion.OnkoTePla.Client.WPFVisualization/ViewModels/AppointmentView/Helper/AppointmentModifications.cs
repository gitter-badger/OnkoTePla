using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using bytePassion.Lib.Communication.State;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Utils;
using bytePassion.OnkoTePla.Client.Core.Domain;
using bytePassion.OnkoTePla.Client.WPFVisualization.Model;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.TherapyPlaceRowView.Helper;
using bytePassion.OnkoTePla.Contracts.Appointments;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

using static bytePassion.OnkoTePla.Client.WPFVisualization.Global.Constants;
using Duration = bytePassion.Lib.TimeLib.Duration;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.AppointmentView.Helper
{
	public class AppointmentModifications : DisposingObject, 
											INotifyPropertyChanged
	{

		private class ModificationDataSet
		{
			public ModificationDataSet(Time beginTime, Time endTime, 
									   string description, TherapyPlaceRowIdentifier location)
			{
				BeginTime = beginTime;
				EndTime = endTime;
				Description = description;
				Location = location;
			}

			public Time BeginTime { get; }
			public Time EndTime { get; }

			public string Description { get; }
			public TherapyPlaceRowIdentifier Location { get; }
		}

		private readonly IDataCenter dataCenter;
		private readonly IViewModelCommunication viewModelCommunication;
		
		private readonly IGlobalState<Date> selectedDateVariable;
		private readonly IGlobalState<Size> gridSizeVariable;

		private readonly VersionManager<ModificationDataSet> versions; 

		private Time beginTime;
		private Time endTime;
		private Time lastSetBeginTime;
		private Time lastSetEndTime;

		private Time currentDayOpeningTime;
		private Time currentDayClosingTime;

		private Time currentSlotBegin;
		private Time currentSlotEnd;

		private MedicalPractice currentMedicalPracticeVersion;
		private double currentGridWidth;				
		private TherapyPlaceRowIdentifier currentLocation;
		private string description;

		public AppointmentModifications(Appointment originalAppointment,										
										Guid medicalPracticeId, 
										IDataCenter dataCenter, 
										IViewModelCommunication viewModelCommunication,
										bool isInitialAdjustment)
		{
			OriginalAppointment = originalAppointment;						
			IsInitialAdjustment = isInitialAdjustment;

			this.dataCenter = dataCenter;
			this.viewModelCommunication = viewModelCommunication;

			versions = new VersionManager<ModificationDataSet>(100);
			versions.CurrentVersionChanged += OnCurrentVersionChanged;
			versions.PropertyChanged += OnVersionsManagerPropertyChanged;

			var aggregateIdentifier = new AggregateIdentifier(originalAppointment.Day, medicalPracticeId);
			var initalLocation = new TherapyPlaceRowIdentifier(aggregateIdentifier, originalAppointment.TherapyPlace.Id);
			
			
			currentMedicalPracticeVersion = dataCenter.GetMedicalPracticeByDateAndId(initalLocation.PlaceAndDate.Date,
																					 initalLocation.PlaceAndDate.MedicalPracticeId);

			var initialDataSet = new ModificationDataSet(originalAppointment.StartTime,
													     originalAppointment.EndTime,
														 originalAppointment.Description,
														 initalLocation);

			versions.AddnewVersion(initialDataSet);

			selectedDateVariable = viewModelCommunication.GetGlobalViewModelVariable<Date>(
				AppointmentGridSelectedDateVariable
			);
			selectedDateVariable.StateChanged += OnSelectedDateVariableChanged;

			gridSizeVariable = viewModelCommunication.GetGlobalViewModelVariable<Size>(
				AppointmentGridSizeVariable	
			);
			gridSizeVariable.StateChanged += OnGridSizeVariableChanged;

			OnGridSizeVariableChanged(gridSizeVariable.Value);
		}

		private void OnVersionsManagerPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			switch (propertyChangedEventArgs.PropertyName)							//
			{																		//
				case nameof(VersionManager<ModificationDataSet>.RedoPossible):		//
				{																	//
					PropertyChanged.Notify(this, nameof(RedoPossible));				//	forwarding event
					break;															//	that notifies
				}																	//	UndoPossible or
				case nameof(VersionManager<ModificationDataSet>.UndoPossible):		//  RedoPossible
				{																	//	has changed
					PropertyChanged.Notify(this, nameof(UndoPossible));				//
					break;															//
				}																	//
			}																		//			
		}

		private void OnCurrentVersionChanged(object sender, ModificationDataSet modificationDataSet)
		{
			ApplyModificationDataset(modificationDataSet);
		}

		public Appointment OriginalAppointment { get; }
		public bool IsInitialAdjustment { get; }

		public double CurrentAppointmentPixelWidth
		{
			get
			{
				var lengthOfOneHour = currentGridWidth / (new Duration(currentDayClosingTime, currentDayOpeningTime).Seconds / 3600.0);
				var duration = new Duration(BeginTime, EndTime);

				return ((double)duration.Seconds / 3600) * lengthOfOneHour;
			}
		}

		private void ApplyModificationDataset(ModificationDataSet dataSet)
		{
			Description = dataSet.Description;

			BeginTime = dataSet.BeginTime;
			EndTime = dataSet.EndTime;

			lastSetBeginTime = BeginTime;
			lastSetEndTime   = EndTime;

			bool recomputationOfSlotsNecessary = CurrentLocation != dataSet.Location;

			CurrentLocation = dataSet.Location;

			if (recomputationOfSlotsNecessary)
				ComputeBoundariesOfCurrentTimeSlot();
		}

		#region Undo/Redo

		public bool UndoPossible { get { return versions.UndoPossible; }}
		public bool RedoPossible { get { return versions.RedoPossible; }}

		public void Undo()
		{
			if (versions.UndoPossible)
				versions.Undo();
		}

		public void Redo()
		{
			if (versions.RedoPossible)
				versions.Redo();
		}

		#endregion

		#region OnGridSizeVariableChanged, OnSelectedDateVariableChanged

		private void OnGridSizeVariableChanged (Size size)
		{
			currentGridWidth = size.Width;
		}

		private void OnSelectedDateVariableChanged (Date date)
		{
			if (date != CurrentLocation.PlaceAndDate.Date)
			{
				var newMedicalPractice = dataCenter.GetMedicalPracticeByDateAndId(date, currentMedicalPracticeVersion.Id);

				if (newMedicalPractice.HoursOfOpening.IsOpen(date))
				{
					var readModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(
						new AggregateIdentifier(date, CurrentLocation.PlaceAndDate.MedicalPracticeId)
						);

					IDictionary<TherapyPlace, IList<Appointment>> sortedAppointments =
						new Dictionary<TherapyPlace, IList<Appointment>>();

					foreach (var therapyPlace in newMedicalPractice.GetAllTherapyPlaces())
						sortedAppointments.Add(therapyPlace, new List<Appointment>());

					foreach (var appointment in readModel.Appointments)
						if (appointment != OriginalAppointment)
							sortedAppointments[appointment.TherapyPlace].Add(appointment);

					var openingTime = newMedicalPractice.HoursOfOpening.GetOpeningTime(date);
					var closingTime = newMedicalPractice.HoursOfOpening.GetClosingTime(date);

					var appointmentDuration = new Duration(BeginTime, EndTime);

					foreach (var therapyRowData in sortedAppointments)
					{
						var slots = ComputeAllSlotsWithinARow(openingTime, closingTime, therapyRowData.Value);
						var suitableSlot = GetSlotForAppointment(slots, appointmentDuration);

						if (suitableSlot != null)
						{
							SetNewLocation(
								new TherapyPlaceRowIdentifier(new AggregateIdentifier(date,
																					  CurrentLocation.PlaceAndDate.MedicalPracticeId),
															  therapyRowData.Key.Id),
								suitableSlot.Begin,
								suitableSlot.Begin + appointmentDuration
							);
							return;
						}
					}

					viewModelCommunication.Send(
						new ShowNotification("cannot move the OriginalAppointment to that day. No timeslot is big enough!")
					);
				}
				else
				{
					viewModelCommunication.Send(
						new ShowNotification("cannot move an OriginalAppointment to a day where the practice is closed!")
					);
				}

				selectedDateVariable.Value = CurrentLocation.PlaceAndDate.Date;
			}
		}

		#endregion

		#region public properties for appointment-changes

		public Time BeginTime
		{
			get { return beginTime; }
			private set { PropertyChanged.ChangeAndNotify(this, ref beginTime, value); }
		}

		public Time EndTime
		{
			get { return endTime; }
			private set { PropertyChanged.ChangeAndNotify(this, ref endTime, value); }
		}		

		public TherapyPlaceRowIdentifier CurrentLocation
		{
			get { return currentLocation; }
			private set { PropertyChanged.ChangeAndNotify(this, ref currentLocation, value); }
		}

		public string Description
		{
			get { return description; }
			private set { PropertyChanged.ChangeAndNotify(this, ref description, value); }
		}

		#endregion

		#region set and fix new values of location / time shift / begin / end / description

		public void SetNewLocation(TherapyPlaceRowIdentifier newLocation, Time newBeginTime, Time newEndTime)
		{			
			var finalBeginTime = GetTimeToSnap(newBeginTime);
			var appointmentDuration = new Duration(newBeginTime, newEndTime);
			var finalEndTime = GetTimeToSnap(finalBeginTime + appointmentDuration);

			if (newLocation != CurrentLocation ||
			    finalBeginTime != BeginTime ||
			    finalEndTime != EndTime)
			{
				versions.AddnewVersion(new ModificationDataSet(finalBeginTime,
				                                               finalEndTime,
				                                               versions.CurrentVersion.Description,
				                                               newLocation));
			}
		}	
		
		public void SetNewTimeShiftDelta(double deltaInPixel)
		{			
			var lengthOfOneHour = currentGridWidth / (new Duration(currentDayClosingTime, currentDayOpeningTime).Seconds / 3600.0);
			var secondsDelta = (uint)Math.Floor(Math.Abs(deltaInPixel) / (lengthOfOneHour / 3600));
			var durationDelta = new Duration(secondsDelta);


			if (deltaInPixel > 0)
			{
				var tempEndTime = CheckEndTime(deltaInPixel > 0 ? lastSetEndTime + durationDelta : lastSetEndTime - durationDelta);
				var actualTimeDelta = new Duration(lastSetEndTime, tempEndTime);
				
				BeginTime = lastSetBeginTime + actualTimeDelta;
				EndTime   = lastSetEndTime + actualTimeDelta;
			}
			else
			{
				var tmpBeginTime = CheckBeginTime(deltaInPixel > 0 ? lastSetBeginTime + durationDelta : lastSetBeginTime - durationDelta);
				var actualTimeDelta = new Duration(lastSetBeginTime, tmpBeginTime);
				
				BeginTime = lastSetBeginTime - actualTimeDelta;
				EndTime   = lastSetEndTime - actualTimeDelta;
			}						
		}

		public void FixTimeShiftDelta()
		{
			var duration = new Duration(BeginTime, EndTime);
			var finalBeginTime = GetTimeToSnap(BeginTime);			
			var finalEndTime = GetTimeToSnap(finalBeginTime + duration);

			if (finalBeginTime != BeginTime ||
			    finalEndTime != EndTime)
			{
				versions.AddnewVersion(new ModificationDataSet(finalBeginTime,
				                                               finalEndTime,
				                                               versions.CurrentVersion.Description,
				                                               versions.CurrentVersion.Location));
			}
		}

		public void SetNewEndTimeDelta(double deltaInPixel)
		{			
			var lengthOfOneHour = currentGridWidth / (new Duration(currentDayClosingTime, currentDayOpeningTime).Seconds / 3600.0);
			var secondsDelta = (uint)Math.Floor(Math.Abs(deltaInPixel) / (lengthOfOneHour / 3600));
			var durationDelta = new Duration(secondsDelta);

			EndTime = CheckEndTime(deltaInPixel > 0 
										? lastSetEndTime + durationDelta 
										: lastSetEndTime - durationDelta);
		}		

		public void FixEndTimeDelta()
		{
			var finalEndTime = GetTimeToSnap(EndTime);

			if (finalEndTime != EndTime)
			{
				versions.AddnewVersion(new ModificationDataSet(versions.CurrentVersion.BeginTime,
				                                               finalEndTime,
				                                               versions.CurrentVersion.Description,
				                                               versions.CurrentVersion.Location));
			}
		}

		public void SetNewBeginTimeDelta(double deltaInPixel)
		{			
			var lengthOfOneHour = currentGridWidth / (new Duration(currentDayClosingTime, currentDayOpeningTime).Seconds / 3600.0);						
			var secondsDelta = (uint)Math.Floor(Math.Abs(deltaInPixel) / (lengthOfOneHour / 3600));
			var durationDelta = new Duration(secondsDelta);

			BeginTime = CheckBeginTime(deltaInPixel > 0 
											? lastSetBeginTime + durationDelta 
											: lastSetBeginTime - durationDelta);			
		}		

		public void FixBeginTimeDelta()
		{
			var finalBeginTime = GetTimeToSnap(BeginTime);

			if (finalBeginTime != BeginTime)
			{
				versions.AddnewVersion(new ModificationDataSet(finalBeginTime,
				                                               versions.CurrentVersion.EndTime,
				                                               versions.CurrentVersion.Description,
				                                               versions.CurrentVersion.Location));
			}
		}

		public void SetNewDescription(string newDescription)
		{
			if (newDescription != Description)
			{
				versions.AddnewVersion(new ModificationDataSet(versions.CurrentVersion.BeginTime,
															   versions.CurrentVersion.EndTime,
															   newDescription,
															   versions.CurrentVersion.Location));
			}
		}

		#endregion

		#region validate funktions: CheckBeginTime, CheckEndTime

		private Time CheckBeginTime (Time beginTimeToCheck)
		{
			if (beginTimeToCheck < currentSlotBegin)
				return currentSlotBegin;

			var minimalBeginTime = EndTime - new Duration(60*15);

			if (beginTimeToCheck > minimalBeginTime)
				return minimalBeginTime;

			return beginTimeToCheck;
		}

		private Time CheckEndTime (Time endTimeToCheck)
		{
			if (endTimeToCheck > currentSlotEnd)
				return currentSlotEnd;

			var minimalTimeEnd = BeginTime + new Duration(60*15);

			if (endTimeToCheck < minimalTimeEnd)
				return minimalTimeEnd;

			return endTimeToCheck;
		}

		#endregion

		#region help functions: GetTimeToSnap, GetSlotForAppointment, ComputeAllSlotsWithinARow, ComputeBoundariesOfCurrentTimeSlot

		private static Time GetTimeToSnap(Time time)
		{
			var m = time.Minute;
			
			if (          m <=  7) return new Time(time.Hour,  0);
			if (m >  7 && m <= 22) return new Time(time.Hour, 15);
			if (m > 22 && m <= 37) return new Time(time.Hour, 30);
			if (m > 37 && m <= 52) return new Time(time.Hour, 45);
			if (m > 52 && m <= 59) return new Time((byte)(time.Hour+1), 0);

			throw new Exception("internal Error");
		}		

		private static TimeSlot GetSlotForAppointment (IEnumerable<TimeSlot> timeSlots, Duration appointmentDuration)
		{
			return timeSlots.FirstOrDefault(slot => new Duration(slot.Begin, slot.End) >= appointmentDuration);
		}

		private static IEnumerable<TimeSlot> ComputeAllSlotsWithinARow (Time openingTime, Time closingTime, 
																		IEnumerable<Appointment> appointments)
		{
			var sortedAppointments = appointments.ToList();
			sortedAppointments.Sort((appointment, appointment1) => appointment.StartTime.CompareTo(appointment1.StartTime));

			var startOfSlots = new List<Time>();
			var endOfSlots = new List<Time>();

			startOfSlots.Add(openingTime);

			foreach (var appointment in sortedAppointments)
			{
				endOfSlots.Add(appointment.StartTime);
				startOfSlots.Add(appointment.EndTime);
			}

			endOfSlots.Add(closingTime);

			var slots = new List<TimeSlot>();

			for (int i = 0; i < startOfSlots.Count; i++)
			{
				slots.Add(new TimeSlot(startOfSlots[i], endOfSlots[i]));
			}

			return slots;
		}

		private void ComputeBoundariesOfCurrentTimeSlot()
		{
			currentMedicalPracticeVersion = dataCenter.GetMedicalPracticeByDateAndId(CurrentLocation.PlaceAndDate.Date,
																					 CurrentLocation.PlaceAndDate.MedicalPracticeId);

			currentDayOpeningTime = currentMedicalPracticeVersion.HoursOfOpening.GetOpeningTime(CurrentLocation.PlaceAndDate.Date);
			currentDayClosingTime = currentMedicalPracticeVersion.HoursOfOpening.GetClosingTime(CurrentLocation.PlaceAndDate.Date);			

			var currentReadModel = dataCenter.ReadModelRepository.GetAppointmentsOfADayReadModel(CurrentLocation.PlaceAndDate);

			var appointmentWithCorrectStartAndEnd = new Appointment(OriginalAppointment.Patient, 
																	OriginalAppointment.Description,
																	OriginalAppointment.TherapyPlace,
																	OriginalAppointment.Day, 
																	BeginTime, 
																	EndTime, 
																	OriginalAppointment.Id);

			var appointmentsWithinTheSameRow = currentReadModel.Appointments
															   .Where(appointment => appointment.TherapyPlace.Id == CurrentLocation.TherapyPlaceId)
															   .Where(appointment => appointment.Id != OriginalAppointment.Id)
															   .Append(appointmentWithCorrectStartAndEnd)
															   .ToList();

			currentReadModel.Dispose();

			appointmentsWithinTheSameRow.Sort((appointment, appointment1) => appointment.StartTime.CompareTo(appointment1.StartTime));
			var indexOfThisAppointment = appointmentsWithinTheSameRow.IndexOf(appointmentWithCorrectStartAndEnd);

			if (appointmentsWithinTheSameRow.Count == 1)
			{
				currentSlotBegin = currentDayOpeningTime;
				currentSlotEnd   = currentDayClosingTime;
			}
			else if (indexOfThisAppointment == 0)
			{
				currentSlotBegin = currentDayOpeningTime;
				currentSlotEnd   = appointmentsWithinTheSameRow[indexOfThisAppointment + 1].StartTime;
			}
			else if (indexOfThisAppointment == appointmentsWithinTheSameRow.Count - 1)
			{
				currentSlotBegin = appointmentsWithinTheSameRow[indexOfThisAppointment - 1].EndTime;
				currentSlotEnd   = currentDayClosingTime;
			}
			else
			{
				currentSlotBegin = appointmentsWithinTheSameRow[indexOfThisAppointment - 1].EndTime;
				currentSlotEnd   = appointmentsWithinTheSameRow[indexOfThisAppointment + 1].StartTime;
			}
		}

		#endregion

		public override void CleanUp()
		{
			selectedDateVariable.StateChanged -= OnSelectedDateVariableChanged;
			gridSizeVariable.StateChanged     -= OnGridSizeVariableChanged;
			versions.CurrentVersionChanged    -= OnCurrentVersionChanged;
			versions.PropertyChanged          -= OnVersionsManagerPropertyChanged;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

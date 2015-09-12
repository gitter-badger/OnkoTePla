﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;
using bytePassion.OnkoTePla.Contracts.Patients;

namespace bytePassion.OnkoTePla.Client.WPFVisualization.TestAndOldStuff.AddAppointmentTestView
{
	public interface IAddAppointmentTestViewModel : IViewModel
	{
		IEnumerable<MedicalPractice> MedicalPractices { get; }
		IEnumerable<User> Users { get; }
		IEnumerable<Patient> Patients { get; }
		ObservableCollection<TherapyPlace> TherapyPlaces { get; }

		MedicalPractice SelectedMedicalPractice { get; set; }
		string          SelectedDateAsString    { get; set; }
		User            SelectedUser            { get; set; }
		Patient         SelectedPatient         { get; set; }
		string          Description             { get; set; }
		string          StartTimeAsString       { get; set; }
		string          EndTimeAsString         { get; set; }
		TherapyPlace    SelectedTherapyPlace    { get; set; }

		ICommand LoadReadModel  { get; }
		ICommand AddAppointment { get; }
	}
}
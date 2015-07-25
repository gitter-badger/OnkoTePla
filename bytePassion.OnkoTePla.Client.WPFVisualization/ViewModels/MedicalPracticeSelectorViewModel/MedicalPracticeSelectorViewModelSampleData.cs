using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelectorViewModel
{
	public class MedicalPracticeSelectorViewModelSampleData : IMedicalPracticeSelectorViewModel
	{
		public MedicalPracticeSelectorViewModelSampleData()
		{
			var med1 = new MedicalPractice(new List<Room>(), "examplePractice 1", 2, new Guid(), null, null);
			var med2 = new MedicalPractice(new List<Room>(), "examplePractice 2", 2, new Guid(), null, null);

			SelectedMedicalPractice = med1;

			AvailableMedicalPractices = new ObservableCollection<MedicalPractice>
			{
				med1,
				med2
			};
		}

		public MedicalPractice SelectedMedicalPractice
		{
			get; set;
		}

		public ObservableCollection<MedicalPractice> AvailableMedicalPractices
		{
			get; private set;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}

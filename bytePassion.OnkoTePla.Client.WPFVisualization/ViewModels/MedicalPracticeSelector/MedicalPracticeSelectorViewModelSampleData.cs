using bytePassion.OnkoTePla.Contracts.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

#pragma warning disable 0067

namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.MedicalPracticeSelector
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

			PracticeIsSelectable = true;
		}

		public MedicalPractice SelectedMedicalPractice { get; set; }
		public ObservableCollection<MedicalPractice> AvailableMedicalPractices { get; }
		public bool PracticeIsSelectable { get; }
		
	    public void Dispose() {	}
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

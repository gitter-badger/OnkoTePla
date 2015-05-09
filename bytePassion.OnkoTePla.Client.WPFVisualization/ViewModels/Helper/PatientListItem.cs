using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using bytePassion.Lib.Extensions;
using bytePassion.OnkoTePla.Contracts.Patients;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper
{
	internal class PatientListItem : INotifyPropertyChanged
	{
		private readonly Patient patient;
		private bool isCurrentVisibleInList;


		public PatientListItem(Patient patient, bool isCurrentVisibleInList = true)
		{
			this.patient = patient;
			this.isCurrentVisibleInList = isCurrentVisibleInList;
		}

		public Patient Patient
		{
			get { return patient; }
		}

		public bool IsCurrentVisibleInList
		{
			get
			{
				return isCurrentVisibleInList;
			}

			set
			{
				PropertyChanged.ChangeAndNotify(this, ref isCurrentVisibleInList, value);
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		public static IReadOnlyList<PatientListItem> ConvertPatientList(IReadOnlyList<Patient> patients)
		{
			return patients.Select(patient => new PatientListItem(patient))
						   .ToList();	
		} 
	}
}
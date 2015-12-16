using bytePassion.OnkoTePla.SampleDataCreation.SampleData;
using System.Windows;


namespace bytePassion.OnkoTePla.SampleDataCreation
{

    public partial class MainWindow
	{
		public MainWindow ()
		{
			InitializeComponent();
		}

		private void ButtonClickCreateConfigTestData     (object sender, RoutedEventArgs e) { ConfigurationData.CreateConfigTestDataAndSaveToJson(); }
		private void ButtonClickLoadConfig               (object sender, RoutedEventArgs e) { ConfigurationData.TestLoad();	                         }

		private void ButtonClickCreateAppointmentTestData(object sender, RoutedEventArgs e) { EventStreamDataBase.GenerateExampleEventStream();      }
		private void ButtonLickLoadAppointments          (object sender, RoutedEventArgs e) { EventStreamDataBase.TestLoad();                        }

		private void ButtonClickCreatePatientTestData    (object sender, RoutedEventArgs e) { PatientDataBase.GenerateJSONPatientsFile(20000);       }	
		private void ButtonClickLoadPatients             (object sender, RoutedEventArgs e) { PatientDataBase.TestLoad();                            }		
	}
}

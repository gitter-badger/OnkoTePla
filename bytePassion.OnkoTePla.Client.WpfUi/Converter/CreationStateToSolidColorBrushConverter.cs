using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog.Helper;
using System;
using System.Globalization;
using System.Windows.Media;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
{
    public class CreationStateToSolidColorBrushConverter : GenericValueConverter<AppointmentCreationState, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(AppointmentCreationState value, CultureInfo culture)
		{
			switch (value)
			{
				case AppointmentCreationState.NoSpaceAvailable:
				case AppointmentCreationState.NoPatientSelected:              return new SolidColorBrush(Colors.Red); 
				case AppointmentCreationState.PatientAndDespriptionAvailable: return new SolidColorBrush(Color.FromRgb(0, 174, 0)); 
				case AppointmentCreationState.PatientSelected:                return new SolidColorBrush(Colors.Orange); 
			}

			throw new Exception("inner exception");
		}
	}
}

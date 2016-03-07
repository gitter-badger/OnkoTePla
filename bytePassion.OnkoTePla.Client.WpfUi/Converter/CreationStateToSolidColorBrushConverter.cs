using System;
using System.Globalization;
using System.Windows.Media;
using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Client.WpfUi.Global;
using bytePassion.OnkoTePla.Client.WpfUi.ViewModels.AddAppointmentDialog.Helper;


namespace bytePassion.OnkoTePla.Client.WpfUi.Converter
{
	internal class CreationStateToSolidColorBrushConverter : GenericValueConverter<AppointmentCreationState, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(AppointmentCreationState value, CultureInfo culture)
		{
			switch (value)
			{
				case AppointmentCreationState.NoSpaceAvailable:
				case AppointmentCreationState.NoPatientSelected:              return new SolidColorBrush(Constants.LayoutColors.AppointmentCreateStateImpossible); 
				case AppointmentCreationState.PatientAndDespriptionAvailable: return new SolidColorBrush(Constants.LayoutColors.AppointmentCreateStatePossibleButNotComplete); 
				case AppointmentCreationState.PatientSelected:                return new SolidColorBrush(Constants.LayoutColors.AppointmentCreateStatePossible); 
			}

			throw new Exception("inner exception");
		}
	}
}

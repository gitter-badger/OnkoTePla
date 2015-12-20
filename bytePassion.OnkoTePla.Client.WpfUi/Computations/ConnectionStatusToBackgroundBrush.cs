using System.Globalization;
using System.Windows.Media;
using bytePassion.Lib.WpfLib.ConverterBase;
using bytePassion.OnkoTePla.Client.WpfUi.Enums;

namespace bytePassion.OnkoTePla.Client.WpfUi.Computations
{
	internal class ConnectionStatusToBackgroundBrush : GenericValueConverter<ConnectionStatus, SolidColorBrush>
	{
		protected override SolidColorBrush Convert(ConnectionStatus connectionStatus, CultureInfo culture)
		{
			switch (connectionStatus)
			{				
				case ConnectionStatus.TryToConnect: 
				case ConnectionStatus.TryToDisconnect: return new SolidColorBrush(Colors.Yellow);	
				case ConnectionStatus.Connected:       return new SolidColorBrush(Colors.LawnGreen); 
				case ConnectionStatus.Disconnected:    return new SolidColorBrush(Colors.Red);
			}

			return new SolidColorBrush(Colors.DimGray);
		}
	}
}

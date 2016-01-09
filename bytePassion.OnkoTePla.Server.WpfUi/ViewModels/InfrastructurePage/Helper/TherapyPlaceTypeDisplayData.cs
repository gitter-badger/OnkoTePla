using System.Windows.Media;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper
{
	internal class TherapyPlaceTypeDisplayData
	{
		public TherapyPlaceTypeDisplayData(string name, ImageSource icon)
		{
			Name = name;
			Icon = icon;
		}

		public string      Name { get; }
		public ImageSource Icon { get; }
	}
}
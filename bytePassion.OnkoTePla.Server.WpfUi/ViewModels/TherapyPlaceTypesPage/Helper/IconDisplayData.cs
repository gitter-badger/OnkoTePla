using System.Windows.Media;
using bytePassion.OnkoTePla.Contracts.Enums;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.TherapyPlaceTypesPage.Helper
{
	internal class IconDisplayData
	{
		public IconDisplayData(ImageSource iconImage, TherapyPlaceIconType iconType, string iconName)
		{
			IconImage = iconImage;
			IconType = iconType;
			IconName = iconName;
		}

		public ImageSource          IconImage { get; }
		public TherapyPlaceIconType IconType  { get; }
		public string               IconName  { get; }
	}
}

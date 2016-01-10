using System;
using System.Windows.Media;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper
{
	internal class TherapyPlaceTypeDisplayData
	{
		public TherapyPlaceTypeDisplayData(string name, ImageSource icon, Guid id)
		{
			Name = name;
			Icon = icon;
			Id = id;
		}

		public string      Name { get; }
		public ImageSource Icon { get; }
		public Guid		   Id   { get; }
	}
}
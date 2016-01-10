using System;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.UserPage.Helper
{
	internal class MedPracticeListItemData
	{
		public MedPracticeListItemData(bool isSelected, string name, Guid id)
		{
			IsSelected = isSelected;
			Name = name;
			Id = id;
		}

		public bool IsSelected { get; set; }
		public string Name { get; }
		public Guid Id { get; }
	}
}

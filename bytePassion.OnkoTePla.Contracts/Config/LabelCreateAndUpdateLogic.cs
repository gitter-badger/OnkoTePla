using System;
using System.Windows.Media;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public static class LabelCreateAndUpdateLogic
	{
		public static Label Create()
		{
			return new Label("noLabelName", Colors.Transparent, Guid.NewGuid());
		}

		public static Label SetNewName(this Label label, string newLabelName)
		{
			return new Label(newLabelName, label.Color, label.Id);
		}

		public static Label SetNewColor(this Label label, Color newLabelColor)
		{
			return new Label(label.Name, newLabelColor, label.Id);
		}
	}
}
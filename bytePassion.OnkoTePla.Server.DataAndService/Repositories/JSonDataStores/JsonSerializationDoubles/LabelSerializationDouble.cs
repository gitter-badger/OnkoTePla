using System;
using System.Windows.Media;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.JSonDataStores.JsonSerializationDoubles
{
	internal class LabelSerializationDouble
	{
		public LabelSerializationDouble()
		{			
		}

		public LabelSerializationDouble(Label label)
		{
			Name   = label.Name;
			Color = label.Color;
			Id    = label.Id;
		}

		public string Name  { get; set; }
		public Color  Color { get; set; }
		public Guid   Id    { get; set; }

		public Label GetLabel()
		{
			return new Label(Name, Color, Id);
		}
	}
}
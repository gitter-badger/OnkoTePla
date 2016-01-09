using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace bytePassion.OnkoTePla.Server.WpfUi.ViewModels.InfrastructurePage.Helper
{
	internal class ColorDisplayData
	{
		public ColorDisplayData(Color color)
		{
			Color = color;
			Name = GetColorName(Color);
		}

		public Color  Color { get; }
		public string Name  { get; }


		private static string GetColorName (Color color)
		{
			return KnownColors.Where(kvp => kvp.Value.Equals(color))
							  .Select(kvp => kvp.Key)
							  .FirstOrDefault();
		}

		private static readonly Dictionary<string, Color> KnownColors = GetKnownColors();

		private static Dictionary<string, Color> GetKnownColors ()
		{
			var colorProperties = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);

			return colorProperties.ToDictionary(p => p.Name,
												p => (Color)p.GetValue(null, null));
		}
	}
}
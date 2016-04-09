using System;
using System.Windows.Media;
using bytePassion.Lib.FrameworkExtensions;

namespace bytePassion.OnkoTePla.Contracts.Config
{
	public class Label
	{
		public Label(string name, Color color, Guid id)
		{
			Name = name;
			Color = color;
			Id = id;
		}

		public string Name  { get; }
		public Color  Color { get; }
		public Guid   Id    { get; }

		public override bool Equals(object obj)
		{
			return this.Equals(obj, (l1, l2) => l1.Id == l2.Id && 
			                                    l1.Color == l2.Color && 
			                                    l1.Name == l2.Name);
		}
		
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Name?.GetHashCode() ?? 0;

				hashCode = (hashCode*397) ^ Color.GetHashCode();
				hashCode = (hashCode*397) ^ Id.GetHashCode();

				return hashCode;
			}
		}

		public static bool operator ==(Label l1, Label l2) => EqualsExtension.EqualsForEqualityOperator(l1, l2);
		public static bool operator !=(Label l1, Label l2) => !(l1 == l2);
	}
}
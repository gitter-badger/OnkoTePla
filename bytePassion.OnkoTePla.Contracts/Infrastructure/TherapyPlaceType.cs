using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Contracts.Enums;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class TherapyPlaceType
	{
        
		public TherapyPlaceType(string name, TherapyPlaceIconType iconType, Guid id)
		{
			Name = name;
			IconType = iconType;
			Id = id;
		}

		public string               Name     { get; }
	    public TherapyPlaceIconType IconType { get; }
	    public Guid                 Id       { get; }
	    

		public override string ToString    ()           => Name;
	    public override bool   Equals      (object obj) => this.Equals(obj, (tpt1, tpt2) => tpt1.Id == tpt2.Id);
	    public override int    GetHashCode ()           => Id.GetHashCode();


		public static bool operator ==(TherapyPlaceType tpt1, TherapyPlaceType tpt2) => EqualsExtension.EqualsForEqualityOperator(tpt1, tpt2);
		public static bool operator !=(TherapyPlaceType tpt1, TherapyPlaceType tpt2) => !(tpt1 == tpt2);
	}
}

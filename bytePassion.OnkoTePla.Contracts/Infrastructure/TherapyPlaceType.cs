﻿using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Contracts.Enums;
using static bytePassion.Lib.FrameworkExtensions.EqualsExtension;


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
	    

		public override string ToString ()
		{
			return Name;
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (tpt1, tpt2) => tpt1.Id == tpt2.Id &&
												    tpt1.IconType == tpt2.IconType &&
													tpt1.Name == tpt2.Name);
		}

		public override int GetHashCode ()
		{
			return Id.GetHashCode();
		}


		public static bool operator ==(TherapyPlaceType tpt1, TherapyPlaceType tpt2) => EqualsForEqualityOperator(tpt1, tpt2);
		public static bool operator !=(TherapyPlaceType tpt1, TherapyPlaceType tpt2) => !(tpt1 == tpt2);
	}
}

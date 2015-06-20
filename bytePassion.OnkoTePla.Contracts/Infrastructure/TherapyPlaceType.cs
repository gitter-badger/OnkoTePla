using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Contracts.Enums;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
	public class TherapyPlaceType
	{
		private readonly Guid id;
		private readonly string name;
		private readonly TherapyPlaceIconType iconType;

		public TherapyPlaceType(string name, TherapyPlaceIconType iconType, Guid id)
		{
			this.name = name;
			this.iconType = iconType;
			this.id = id;
		}

		public string               Name     { get { return name;     }}
		public TherapyPlaceIconType IconType { get { return iconType; }}
		public Guid                 Id       { get { return id;       }}	
	
		#region ToString / HashCode / Equals

		public override string ToString ()
		{
			return Name;
		}

		public override bool Equals (object obj)
		{
			return this.Equals(obj, (tpt1, tpt2) => tpt1.Id == tpt2.Id);
		}

		public override int GetHashCode ()
		{
			return id.GetHashCode();
		}

		#endregion	
	}
}

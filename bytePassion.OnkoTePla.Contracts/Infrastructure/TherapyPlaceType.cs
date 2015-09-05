using System;
using System.Runtime.Serialization;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Contracts.Enums;


namespace bytePassion.OnkoTePla.Contracts.Infrastructure
{
    [DataContract]
	public class TherapyPlaceType
	{

        [DataMember(Name = "Id")]       private readonly Guid id;
        [DataMember(Name = "Name")]     private readonly string name;
        [DataMember(Name = "IconType")] private readonly TherapyPlaceIconType iconType;

		public TherapyPlaceType(string name, TherapyPlaceIconType iconType, Guid id)
		{
			this.name = name;
			this.iconType = iconType;
			this.id = id;
		}

		public string               Name     => name;
	    public TherapyPlaceIconType IconType => iconType;
	    public Guid                 Id       => id;
	    

		public override string ToString    ()           => Name;
	    public override bool   Equals      (object obj) => this.Equals(obj, (tpt1, tpt2) => tpt1.Id == tpt2.Id);
	    public override int    GetHashCode ()           => id.GetHashCode();
	}
}

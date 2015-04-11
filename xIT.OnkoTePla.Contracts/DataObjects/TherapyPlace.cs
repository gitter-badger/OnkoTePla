namespace xIT.OnkoTePla.Contracts.DataObjects
{
	public sealed class TherapyPlace
	{
		private readonly uint             therapyPlaceID;
		private readonly TherapyPlaceType therapyPlaceType;

		public TherapyPlace(uint therapyPlaceID, TherapyPlaceType therapyPlaceType)
		{
			this.therapyPlaceID   = therapyPlaceID;
			this.therapyPlaceType = therapyPlaceType;
		}

		public uint TherapyPlaceID
		{
			get { return therapyPlaceID; }
		}

		public TherapyPlaceType TherapyPlaceType
		{
			get { return therapyPlaceType; }
		}
	}
}

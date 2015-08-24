using System;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.OnkoTePla.Client.Core.Domain;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Helper
{
	public class AppointmentLocalisation
	{
		public AppointmentLocalisation(AggregateIdentifier placeAndDate, 
									   Guid therapyPlaceRowId)
		{
			PlaceAndDate      = placeAndDate;
			TherapyPlaceRowId = therapyPlaceRowId;
		}

		public AggregateIdentifier PlaceAndDate { get; }
		public Guid TherapyPlaceRowId { get; }

		public override bool Equals(object obj)
		{
			return this.Equals(obj,
							  (al1, al2) => al1.PlaceAndDate == al2.PlaceAndDate && 
										    al1.TherapyPlaceRowId == al2.TherapyPlaceRowId);
		}

		public override int GetHashCode()
		{
			return PlaceAndDate.GetHashCode() ^ 
				   TherapyPlaceRowId.GetHashCode();
		}

		public override string ToString()
		{
			return $"[{PlaceAndDate}|{TherapyPlaceRowId}]";
        }

		public static bool operator == (AppointmentLocalisation al1, AppointmentLocalisation al2) => al1.Equals(al2);
		public static bool operator != (AppointmentLocalisation al1, AppointmentLocalisation al2) => !(al1 == al2);
	}
}

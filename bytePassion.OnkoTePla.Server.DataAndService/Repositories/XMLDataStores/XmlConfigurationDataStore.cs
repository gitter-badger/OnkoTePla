using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml;
using bytePassion.Lib.TimeLib;
using bytePassion.Lib.Types.Repository;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Infrastructure;

namespace bytePassion.OnkoTePla.Server.DataAndService.Repositories.XMLDataStores
{
	public class XmlConfigurationDataStore : IPersistenceService<Configuration>
	{
		private readonly string filename;

		public XmlConfigurationDataStore(string filename)
		{
			this.filename = filename;
		}


		private static XmlWriterSettings WriterSettings
		{
			get
			{
				return new XmlWriterSettings
				{
					Indent = true,
					IndentChars = "  ",
					NewLineChars = "\r\n",
					NewLineHandling = NewLineHandling.Replace
				};
			}
		}

		#region xml node- and attribute names
		private const string XmlRoot = "configuration";

		private const string CountAttribute = "count";
		private const string NameAttribute  = "name";
		private const string IdAttribute    = "id";

		private const string TherapyPlaceTypes = "therapyPlaceTypes";		
		private const string TherapyPlaceType  = "therapyPlaceType";

		private const string MedicalPractices = "medicalPractices";
		private const string MedicalPractice  = "medicalPractice";

		private const string Users = "users";
		private const string User  = "user";
		
		private const string Room                 = "room";
		private const string TherapyPlace         = "therapyPlace";
		private const string HoursOfOpening       = "hoursOfOpening";
		private const string AdditionalOpenedDays  = "additionalOpendDays";
		private const string AdditionalClosedDays = "additionalClosedDays";
		private const string Day                  = "day";

	    private const string IsHiddenAttribute            = "isHidden";
		private const string IconTypeAttribute            = "iconType";			
		private const string MPVersionAttribute           = "version";						
		private const string TherapyPlaceTypeAttribute    = "type";		
		private const string PassworAttribute             = "password";
		private const string AccessablePratice            = "accessablePractice";
		private const string HasPreviousVersionsAttribute = "hasPreviousVersions";
		private const string ColorAttribute               = "color";

		private const string DateAttribute = "date";

		private const string OpeningTimeMondayAttribute    = "openingTimeMonday";
		private const string OpeningTimeTuesdayAttribute   = "openingTimeTuesday";
		private const string OpeningTimeWednesdayAttribute = "openingTimeWednesday";
		private const string OpeningTimeThursdayAttribute  = "openingTimeThursday";
		private const string OpeningTimeFridayAttribute    = "openingTimeFriday";
		private const string OpeningTimeSaturdayAttribute  = "openingTimeSaturday";
		private const string OpeningTimeSundayAttribute    = "openingTimeSunday";

		private const string ClosingTimeMondayAttribute    = "closingTimeMonday";
		private const string ClosingTimeTuesdayAttribute   = "closingTimeTuesday";
		private const string ClosingTimeWednesdayAttribute = "closingTimeWednesday";
		private const string ClosingTimeThursdayAttribute  = "closingTimeThursday";
		private const string ClosingTimeFridayAttribute    = "closingTimeFriday";
		private const string ClosingTimeSaturdayAttribute  = "closingTimeSaturday";
		private const string ClosingTimeSundayAttribute    = "closingTimeSunday";

		private const string IsOpenOnMondayAttribute    = "isOpenOnMonday";
		private const string IsOpenOnTuesdayAttribute   = "isOpenOnTuesday";
		private const string IsOpenOnWednesdayAttribute = "isOpenOnWednesday";
		private const string IsOpenOnThursdayAttribute  = "isOpenOnThursday";
		private const string IsOpenOnFridayAttribute    = "isOpenOnFriday";
		private const string IsOpenOnSaturdayAttribute  = "isOpenOnSaturday";
		private const string IsOpenOnSundayAttribute    = "isOpenOnSunday";
		#endregion

		public void Persist(Configuration config)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();

				writer.WriteStartElement(XmlRoot);

					writer.WriteStartElement(TherapyPlaceTypes);
					writer.WriteAttributeString(CountAttribute, config.GetAllTherapyPlaceTypes().Count().ToString());
					foreach (var therapyPlaceType in config.GetAllTherapyPlaceTypes())
					{
						WriteTherapyPlaceType(writer, therapyPlaceType);
					}
					writer.WriteEndElement();

					writer.WriteStartElement(MedicalPractices);
					writer.WriteAttributeString(CountAttribute, config.GetAllMedicalPractices().Count().ToString());
					foreach (var medicalPractice in config.GetAllMedicalPractices())
					{
						WriteMedicalPractice(writer, medicalPractice);
					}					
					writer.WriteEndElement();

					writer.WriteStartElement(Users);
					writer.WriteAttributeString(CountAttribute, config.GetAllUsers().Count().ToString());
					foreach (var user in config.GetAllUsers())
					{
						WriteUser(writer, user);
					}
					writer.WriteEndElement();
				writer.WriteEndElement();

			writer.WriteEndDocument();
			writer.Close();
		}

		#region writer methods
		private static void WriteUser(XmlWriter writer, User user)
		{
			writer.WriteStartElement(User);
			writer.WriteAttributeString(NameAttribute, user.Name);
			writer.WriteAttributeString(PassworAttribute, user.Password);
			writer.WriteAttributeString(IdAttribute, user.Id.ToString());
			writer.WriteAttributeString(IsHiddenAttribute, user.IsHidden.ToString());

			var index = 0;
			foreach (var id in user.ListOfAccessableMedicalPractices)
			{
				writer.WriteAttributeString(AccessablePratice + (index++), id.ToString());
			}
			

			writer.WriteEndElement();
		}
		
		private static void WriteMedicalPractice(XmlWriter writer, MedicalPractice medicalPractice)
		{
			writer.WriteStartElement(MedicalPractice);
			writer.WriteAttributeString(NameAttribute, medicalPractice.Name);	
			writer.WriteAttributeString(CountAttribute, medicalPractice.Rooms.Count().ToString());
			writer.WriteAttributeString(MPVersionAttribute, medicalPractice.Version.ToString());
			writer.WriteAttributeString(IdAttribute, medicalPractice.Id.ToString());
			writer.WriteAttributeString(HasPreviousVersionsAttribute, medicalPractice.HasPreviousVersion.ToString());

			foreach (var room in medicalPractice.Rooms)
			{
				WriteRoom(writer, room);
			}

			WriteHoursOfOpening(writer, medicalPractice.HoursOfOpening);

			if (medicalPractice.HasPreviousVersion)
				WriteMedicalPractice(writer, medicalPractice.PreviousVersion);

			writer.WriteEndElement();
		}

		private static void WriteHoursOfOpening(XmlWriter writer, HoursOfOpening hoursOfOpening)
		{
			writer.WriteStartElement(HoursOfOpening);

			writer.WriteAttributeString(OpeningTimeMondayAttribute,    hoursOfOpening.OpeningTimeMonday.ToString());
			writer.WriteAttributeString(OpeningTimeTuesdayAttribute,   hoursOfOpening.OpeningTimeTuesday.ToString());
			writer.WriteAttributeString(OpeningTimeWednesdayAttribute, hoursOfOpening.OpeningTimeWednesday.ToString());
			writer.WriteAttributeString(OpeningTimeThursdayAttribute,  hoursOfOpening.OpeningTimeThursday.ToString());
			writer.WriteAttributeString(OpeningTimeFridayAttribute,    hoursOfOpening.OpeningTimeFriday.ToString());
			writer.WriteAttributeString(OpeningTimeSaturdayAttribute,  hoursOfOpening.OpeningTimeSaturday.ToString());
			writer.WriteAttributeString(OpeningTimeSundayAttribute,    hoursOfOpening.OpeningTimeSunday.ToString());

			writer.WriteAttributeString(ClosingTimeMondayAttribute,    hoursOfOpening.ClosingTimeMonday.ToString());
			writer.WriteAttributeString(ClosingTimeTuesdayAttribute,   hoursOfOpening.ClosingTimeTuesday.ToString());
			writer.WriteAttributeString(ClosingTimeWednesdayAttribute, hoursOfOpening.ClosingTimeWednesday.ToString());
			writer.WriteAttributeString(ClosingTimeThursdayAttribute,  hoursOfOpening.ClosingTimeThursday.ToString());
			writer.WriteAttributeString(ClosingTimeFridayAttribute,    hoursOfOpening.ClosingTimeFriday.ToString());
			writer.WriteAttributeString(ClosingTimeSaturdayAttribute,  hoursOfOpening.ClosingTimeSaturday.ToString());
			writer.WriteAttributeString(ClosingTimeSundayAttribute,    hoursOfOpening.ClosingTimeSunday.ToString());

			writer.WriteAttributeString(IsOpenOnMondayAttribute,    hoursOfOpening.IsOpenOnMonday.ToString());
			writer.WriteAttributeString(IsOpenOnTuesdayAttribute,   hoursOfOpening.IsOpenOnTuesday.ToString());
			writer.WriteAttributeString(IsOpenOnWednesdayAttribute, hoursOfOpening.IsOpenOnWednesday.ToString());
			writer.WriteAttributeString(IsOpenOnThursdayAttribute,  hoursOfOpening.IsOpenOnThursday.ToString());
			writer.WriteAttributeString(IsOpenOnFridayAttribute,    hoursOfOpening.IsOpenOnFriday.ToString());
			writer.WriteAttributeString(IsOpenOnSaturdayAttribute,  hoursOfOpening.IsOpenOnSaturday.ToString());
			writer.WriteAttributeString(IsOpenOnSundayAttribute,    hoursOfOpening.IsOpenOnSunday.ToString());

			WriteAdditionalOpenedDays(writer, hoursOfOpening.AdditionalOpenedDays);
			WriteAdditionalClosedDays(writer, hoursOfOpening.AdditionalClosedDays);

			writer.WriteEndElement();
		}

		private static void WriteAdditionalClosedDays(XmlWriter writer, IReadOnlyList<Date> additionalClosedDays)
		{
			writer.WriteStartElement(AdditionalClosedDays);
			writer.WriteAttributeString(CountAttribute, additionalClosedDays.Count.ToString());

			foreach (var day in additionalClosedDays)
			{
				WriteDay(writer, day);
			}

			writer.WriteEndElement();
		}

		private static void WriteAdditionalOpenedDays(XmlWriter writer, IReadOnlyList<Date> additionalOpenedDays)
		{
			writer.WriteStartElement(AdditionalOpenedDays);
			writer.WriteAttributeString(CountAttribute, additionalOpenedDays.Count.ToString());

			foreach (var day in additionalOpenedDays)
			{
				WriteDay(writer, day);
			}

			writer.WriteEndElement();
		}

		private static void WriteDay(XmlWriter writer, Date day)
		{
			writer.WriteStartElement(Day);
			writer.WriteAttributeString(DateAttribute, day.ToString());
			writer.WriteEndElement();
		}

		private static void WriteRoom(XmlWriter writer, Room room)
		{
			writer.WriteStartElement(Room);
			writer.WriteAttributeString(IdAttribute, room.Id.ToString());
			writer.WriteAttributeString(CountAttribute, room.TherapyPlaces.Count().ToString());
			writer.WriteAttributeString(NameAttribute, room.Name);
			writer.WriteAttributeString(ColorAttribute, room.DisplayedColor.ToString());

			foreach (var therapyPlace in room.TherapyPlaces)
			{
				WriteTherapyPlace(writer, therapyPlace);
			}

			writer.WriteEndElement();
		}

		private static void WriteTherapyPlace(XmlWriter writer, TherapyPlace therapyPlace)
		{
			writer.WriteStartElement(TherapyPlace);

			writer.WriteAttributeString(IdAttribute, therapyPlace.Id.ToString());
			writer.WriteAttributeString(NameAttribute, therapyPlace.Name);
			writer.WriteAttributeString(TherapyPlaceTypeAttribute, therapyPlace.TypeId.ToString());
			
			writer.WriteEndElement();
		}

		private static void WriteTherapyPlaceType(XmlWriter writer, TherapyPlaceType therapyPlaceType)
		{
			writer.WriteStartElement(TherapyPlaceType);

			writer.WriteAttributeString(NameAttribute, therapyPlaceType.Name);
			writer.WriteAttributeString(IconTypeAttribute, therapyPlaceType.IconType.ToString());
			writer.WriteAttributeString(IdAttribute, therapyPlaceType.Id.ToString());

			writer.WriteEndElement();
		} 
		#endregion

		Configuration IPersistenceService<Configuration>.Load()
		{
			if (!File.Exists(filename))
			{
				return new Configuration(new List<TherapyPlaceType>(), new List<MedicalPractice>(), new List<User>());
			}

			IEnumerable<TherapyPlaceType> therapyPlaceTypes = null;
			IList<MedicalPractice> practices = new List<MedicalPractice>();
			IList<User> users = new List<User>();

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == TherapyPlaceTypes)
					{

						if (!reader.HasAttributes) continue;

						var therapyPlaceTypesCount = 0;

						while (reader.MoveToNextAttribute())
						{
							if (reader.Name == CountAttribute)
								therapyPlaceTypesCount = Int32.Parse(reader.Value);
						}

						therapyPlaceTypes = AcceptTherapyPlaceTypes(reader, therapyPlaceTypesCount);						
					}

					if (reader.NodeType == XmlNodeType.Element && reader.Name == MedicalPractices)
					{

						if (!reader.HasAttributes) continue;

						var medicalPracticesCount = 0;

						while (reader.MoveToNextAttribute())
						{
							if (reader.Name == CountAttribute)
								medicalPracticesCount = Int32.Parse(reader.Value);
						}

						practices = AcceptMedicalPractices(reader, medicalPracticesCount);
					}

					if (reader.NodeType == XmlNodeType.Element && reader.Name == Users)
					{

						if (!reader.HasAttributes) continue;

						var userCount = 0;

						while (reader.MoveToNextAttribute())
						{
							if (reader.Name == CountAttribute)
								userCount = Int32.Parse(reader.Value);
						}

						users = AcceptUsers(reader, userCount);						
					}
				}
			}
			reader.Close();

			return new Configuration(therapyPlaceTypes, practices, users);
		}
		
		#region readerMethods
		private IList<User> AcceptUsers(XmlReader reader, int userCount)
		{
			IList<User> users = new List<User>();

			var i = 0;
			while (i < userCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != User) continue;
				i++;

				var name = string.Empty;
				var id = new Guid();
				var password = string.Empty;				
				var indexOfAccessablePractices = 0;
				var listOfAccessableMedicalPractices = new List<Guid>();
				var isHidden = false;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == NameAttribute) name = reader.Value;
						if (reader.Name == PassworAttribute) password = reader.Value;
						if (reader.Name == IdAttribute) id = Guid.Parse(reader.Value);
						if (reader.Name == IsHiddenAttribute) isHidden = bool.Parse(reader.Value);

						if (reader.Name == AccessablePratice + indexOfAccessablePractices)
						{
							indexOfAccessablePractices++;
							listOfAccessableMedicalPractices.Add(Guid.Parse(reader.Value));
						}
					}
				}

				users.Add(new User(name, listOfAccessableMedicalPractices, password, id, isHidden));
			}

			return users;
		}

		
		private static IList<MedicalPractice> AcceptMedicalPractices(XmlReader reader, int medicalPracticesCount)
		{
			IList<MedicalPractice> medicalPractices = new List<MedicalPractice>();

			var i = 0;
			while (i < medicalPracticesCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != MedicalPractice) continue;
				i++;

				var medicalPractice = AcceptMedicalPractice(reader);				
				medicalPractices.Add(medicalPractice);
			}
			return medicalPractices;			
		}

		private static MedicalPractice AcceptMedicalPractice (XmlReader reader)
		{
			var name = String.Empty;
			var version = 0u;
			var roomCount = 0;
			var id = new Guid();
			var hasPreviousVersion = false;

			if (reader.HasAttributes)
			{
				while (reader.MoveToNextAttribute())
				{
					if (reader.Name == NameAttribute)                name               = reader.Value;
					if (reader.Name == MPVersionAttribute)           version            = UInt32.Parse(reader.Value);
					if (reader.Name == CountAttribute)               roomCount          = Int32.Parse(reader.Value);
					if (reader.Name == IdAttribute)                  id                 = Guid.Parse(reader.Value);
					if (reader.Name == HasPreviousVersionsAttribute) hasPreviousVersion = Boolean.Parse(reader.Value);
				}
			}

			var rooms = AcceptRooms(reader, roomCount);

			var hoursOfOpening = AcceptHoursOfOpening(reader);

			MedicalPractice previousVersion = null;
			if (hasPreviousVersion)
			{
				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != MedicalPractice) continue;
					previousVersion = AcceptMedicalPractice(reader);
					break;
				}
			}

			return new MedicalPractice(rooms, name, version,id, previousVersion, hoursOfOpening);
		}

		private static HoursOfOpening AcceptHoursOfOpening(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != HoursOfOpening) continue;

				var openingTimeMonday    = Time.Dummy;
				var openingTimeTuesday   = Time.Dummy;
				var openingTimeWednesday = Time.Dummy;
				var openingTimeThursday  = Time.Dummy;
				var openingTimeFriday    = Time.Dummy;
				var openingTimeSaturday  = Time.Dummy;
				var openingTimeSunday    = Time.Dummy;

				var closingTimeMonday    = Time.Dummy;
				var closingTimeTuesday   = Time.Dummy;
				var closingTimeWednesday = Time.Dummy;
				var closingTimeThursday  = Time.Dummy;
				var closingTimeFriday    = Time.Dummy;
				var closingTimeSaturday  = Time.Dummy;
				var closingTimeSunday    = Time.Dummy;

				var isOpenOnMonday    = false;
				var isOpenOnTuesday   = false;
				var isOpenOnWednesday = false;
				var isOpenOnThursday  = false;
				var isOpenOnFriday    = false;
				var isOpenOnSaturday  = false;
				var isOpenOnSunday    = false;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						switch (reader.Name)
						{
							case OpeningTimeMondayAttribute:    openingTimeMonday    = Time.Parse(reader.Value); break;
							case OpeningTimeTuesdayAttribute:   openingTimeTuesday   = Time.Parse(reader.Value); break;
							case OpeningTimeWednesdayAttribute: openingTimeWednesday = Time.Parse(reader.Value); break;
							case OpeningTimeThursdayAttribute:  openingTimeThursday  = Time.Parse(reader.Value); break;
							case OpeningTimeFridayAttribute:    openingTimeFriday    = Time.Parse(reader.Value); break;
							case OpeningTimeSaturdayAttribute:  openingTimeSaturday  = Time.Parse(reader.Value); break;
							case OpeningTimeSundayAttribute:    openingTimeSunday    = Time.Parse(reader.Value); break;

							case ClosingTimeMondayAttribute:    closingTimeMonday    = Time.Parse(reader.Value); break;
							case ClosingTimeTuesdayAttribute:   closingTimeTuesday   = Time.Parse(reader.Value); break;
							case ClosingTimeWednesdayAttribute: closingTimeWednesday = Time.Parse(reader.Value); break;
							case ClosingTimeThursdayAttribute:  closingTimeThursday  = Time.Parse(reader.Value); break;
							case ClosingTimeFridayAttribute:    closingTimeFriday    = Time.Parse(reader.Value); break;
							case ClosingTimeSaturdayAttribute:  closingTimeSaturday  = Time.Parse(reader.Value); break;
							case ClosingTimeSundayAttribute:    closingTimeSunday    = Time.Parse(reader.Value); break;

							case IsOpenOnMondayAttribute:    isOpenOnMonday    = Boolean.Parse(reader.Value); break;
							case IsOpenOnTuesdayAttribute:   isOpenOnTuesday   = Boolean.Parse(reader.Value); break;
							case IsOpenOnWednesdayAttribute: isOpenOnWednesday = Boolean.Parse(reader.Value); break;
							case IsOpenOnThursdayAttribute:  isOpenOnThursday  = Boolean.Parse(reader.Value); break;
							case IsOpenOnFridayAttribute:    isOpenOnFriday    = Boolean.Parse(reader.Value); break;
							case IsOpenOnSaturdayAttribute:  isOpenOnSaturday  = Boolean.Parse(reader.Value); break;
							case IsOpenOnSundayAttribute:    isOpenOnSunday    = Boolean.Parse(reader.Value); break;
						}
					}
				}

				var additionalOpenedDays = AcceptAdditionalOpenedDays(reader);
				var additionalClosedDays = AcceptAdditionalClosedDays(reader);

				return new HoursOfOpening(openingTimeMonday, openingTimeTuesday, openingTimeWednesday, openingTimeThursday, 
							  openingTimeFriday, openingTimeSaturday, openingTimeSunday, 
							  closingTimeMonday, closingTimeTuesday, closingTimeWednesday, closingTimeThursday,
							  closingTimeFriday, closingTimeSaturday, closingTimeSunday, 
							  isOpenOnMonday, isOpenOnTuesday, isOpenOnWednesday, isOpenOnThursday, 
							  isOpenOnFriday, isOpenOnSaturday, isOpenOnSunday, 
							  additionalClosedDays, additionalOpenedDays);
				
			}

			throw new XmlException();
		}

		private static IReadOnlyList<Date> AcceptAdditionalOpenedDays(XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != AdditionalOpenedDays) continue;

				int dayCount = 0;

				if (reader.HasAttributes)				
					while (reader.MoveToNextAttribute())					
						if (reader.Name == CountAttribute) dayCount = Int32.Parse(reader.Value);						
									
				return AcceptDayList(reader, dayCount);
			}
			throw new XmlException();
		}		

		private static IReadOnlyList<Date> AcceptAdditionalClosedDays (XmlReader reader)
		{
			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != AdditionalClosedDays) continue;

				int dayCount = 0;

				if (reader.HasAttributes)
					while (reader.MoveToNextAttribute())
						if (reader.Name == CountAttribute) dayCount = Int32.Parse(reader.Value);

				return AcceptDayList(reader, dayCount);
			}
			throw new XmlException();
		}

		private static IReadOnlyList<Date> AcceptDayList (XmlReader reader, int dayCount)
		{
			var days = new List<Date>();

			var i = 0;
			while (i < dayCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != Day) continue;
				i++;

				var date = Date.Dummy;

				if (reader.HasAttributes)
					while (reader.MoveToNextAttribute())
						if (reader.Name == DateAttribute) date = Date.Parse(reader.Value);

				days.Add(date);
			}
			return days;			
		}

		private static IReadOnlyList<Room> AcceptRooms (XmlReader reader, int roomCount)
		{
			IList<Room> rooms = new List<Room>();

			var i = 0;
			while (i < roomCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != Room) continue;
				i++;
				
				var id = new Guid();
				var name = String.Empty;
				var placeCount = 0;
				var color = new Color();

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == IdAttribute)    id         = Guid.Parse(reader.Value);
						if (reader.Name == CountAttribute) placeCount = Int32.Parse(reader.Value);
						if (reader.Name == NameAttribute)  name       = reader.Value;
						if (reader.Name == ColorAttribute) color      = (Color)ColorConverter.ConvertFromString(reader.Value);
					}
				}

				var therapyPlaces = AcceptTherapyPlace(reader, placeCount);
				rooms.Add(new Room(id, name, therapyPlaces, color));
			}

			return rooms.ToList();
		}

		private static IReadOnlyList<TherapyPlace> AcceptTherapyPlace(XmlReader reader, int placeCount)
		{
			IList<TherapyPlace> therapyPlaces  = new List<TherapyPlace>();

			var i = 0;
			while (i < placeCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != TherapyPlace) continue;
				i++;

				var id = new Guid();
				var typeId = new Guid();
				var name = String.Empty;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == IdAttribute)               id     = Guid.Parse(reader.Value);
						if (reader.Name == TherapyPlaceTypeAttribute) typeId = Guid.Parse(reader.Value);
						if (reader.Name == NameAttribute)             name   = reader.Value;
					}
				}

				var therapyPlace = new TherapyPlace(id, typeId, name);
				therapyPlaces.Add(therapyPlace);
			}

			return therapyPlaces.ToList();
		}

		private static IEnumerable<TherapyPlaceType> AcceptTherapyPlaceTypes(XmlReader reader, int therapyPlaceTypesCount)
		{

			IList<TherapyPlaceType> therapyPlaceTypes = new List<TherapyPlaceType>();

			var i = 0;
			while (i < therapyPlaceTypesCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != TherapyPlaceType) continue;
				i++;

				var name     = String.Empty;
				var iconType = TherapyPlaceTypeIcon.BedType1;
				var id = new Guid();

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == NameAttribute)     name     = reader.Value;
						if (reader.Name == IconTypeAttribute) iconType = (TherapyPlaceTypeIcon)Enum.Parse(typeof(TherapyPlaceTypeIcon),reader.Value);
						if (reader.Name == IdAttribute)       id       = Guid.Parse(reader.Value);
					}
				}

				therapyPlaceTypes.Add(new TherapyPlaceType(name, iconType, id));
			}
			return therapyPlaceTypes;
		}
		#endregion
	}
}

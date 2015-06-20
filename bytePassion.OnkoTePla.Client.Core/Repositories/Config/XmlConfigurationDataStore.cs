using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Enums;
using bytePassion.OnkoTePla.Contracts.Infrastructure;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.Config
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

		private const string Room         = "room";
		private const string TherapyPlace = "therapyPlace";

		private const string IconTypeAttribute            = "iconType";			
		private const string MPVersionAttribute           = "version";						
		private const string TherapyPlaceTypeAttribute    = "type";		
		private const string PassworAttribute             = "password";
		private const string AccessablePratice            = "accessablePractice";
		private const string HasPreviousVersionsAttribute = "hasPreviousVersions";
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

		#region 
		private void WriteUser(XmlWriter writer, User user)
		{
			writer.WriteStartElement(User);
			writer.WriteAttributeString(NameAttribute, user.Name);
			writer.WriteAttributeString(PassworAttribute, user.Password);
			writer.WriteAttributeString(IdAttribute, user.Id.ToString());

			int index = 0;
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

			if (medicalPractice.HasPreviousVersion)
				WriteMedicalPractice(writer, medicalPractice.PreviousVersion);

			writer.WriteEndElement();
		}

		private static void WriteRoom(XmlWriter writer, Room room)
		{
			writer.WriteStartElement(Room);
			writer.WriteAttributeString(IdAttribute, room.Id.ToString());
			writer.WriteAttributeString(CountAttribute, room.TherapyPlaces.Count.ToString());
			writer.WriteAttributeString(NameAttribute, room.Name);

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
			writer.WriteAttributeString(TherapyPlaceTypeAttribute, therapyPlace.Type.Id.ToString());
			
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
			IDictionary<Guid, TherapyPlaceType> therapyPlaceTypes = new Dictionary<Guid, TherapyPlaceType>();
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

						var types = AcceptTherapyPlaceTypes(reader, therapyPlaceTypesCount);
						therapyPlaceTypes = types.ToDictionary(type => type.Id, type => type);
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

						practices = AcceptMedicalPractices(reader, therapyPlaceTypes, medicalPracticesCount);
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

			return new Configuration(therapyPlaceTypes.Values.ToList(), practices, users);
		}
		
		#region readerMethods
		private IList<User> AcceptUsers(XmlReader reader, int userCount)
		{
			IList<User> users = new List<User>();

			int i = 0;
			while (i < userCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != User) continue;
				i++;

				var name = String.Empty;
				var id = new Guid();
				var password = String.Empty;				
				var indexOfAccessablePractices = 0;
				var listOfAccessableMedicalPractices = new List<Guid>();

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == NameAttribute) name = reader.Value;
						if (reader.Name == PassworAttribute) password = reader.Value;
						if (reader.Name == IdAttribute) id = Guid.Parse(reader.Value);

						if (reader.Name == AccessablePratice + indexOfAccessablePractices)
						{
							indexOfAccessablePractices++;
							listOfAccessableMedicalPractices.Add(Guid.Parse(reader.Value));
						}
					}
				}

				users.Add(new User(name, listOfAccessableMedicalPractices, password, id));
			}

			return users;
		}

		
		private static IList<MedicalPractice> AcceptMedicalPractices(XmlReader reader, 
																	 IDictionary<Guid, TherapyPlaceType> therapyPlaceTypes, 
																	 int medicalPracticesCount)
		{
			IList<MedicalPractice> medicalPractices = new List<MedicalPractice>();

			int i = 0;
			while (i < medicalPracticesCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != MedicalPractice) continue;
				i++;

				var medicalPractice = AcceptMedicalPractice(reader, therapyPlaceTypes);				
				medicalPractices.Add(medicalPractice);
			}
			return medicalPractices;			
		}

		private static MedicalPractice AcceptMedicalPractice (XmlReader reader,
															 IDictionary<Guid, TherapyPlaceType> therapyPlaceTypes)
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

			var rooms = AcceptRooms(reader, therapyPlaceTypes, roomCount);

			MedicalPractice previousVersion = null;
			if (hasPreviousVersion)
			{
				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != MedicalPractice) continue;
					previousVersion = AcceptMedicalPractice(reader, therapyPlaceTypes);
					break;
				}
			}

			return new MedicalPractice(rooms, name, version,id, previousVersion);
		}

		private static IReadOnlyList<Room> AcceptRooms (XmlReader reader, 
														IDictionary<Guid, TherapyPlaceType> therapyPlaceTypes, 
														int roomCount)
		{
			IList<Room> rooms = new List<Room>();

			int i = 0;
			while (i < roomCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != Room) continue;
				i++;
				
				var id = new Guid();
				var name = String.Empty;
				var placeCount = 0;

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == IdAttribute)                id         = Guid.Parse(reader.Value);
						if (reader.Name == CountAttribute) placeCount = Int32.Parse(reader.Value);
						if (reader.Name == NameAttribute) name = reader.Value;

					}
				}

				IReadOnlyList<TherapyPlace> therapyPlaces = AcceptTherapyPlace(reader, therapyPlaceTypes, placeCount);
				rooms.Add(new Room(id, name, therapyPlaces));
			}

			return rooms.ToList();
		}

		private static IReadOnlyList<TherapyPlace> AcceptTherapyPlace(XmlReader reader, 
																	  IDictionary<Guid, TherapyPlaceType> therapyPlaceTypes, 
																	  int placeCount)
		{
			IList<TherapyPlace> therapyPlaces  = new List<TherapyPlace>();

			int i = 0;
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

				var therapyPlace = new TherapyPlace(id, therapyPlaceTypes[typeId], name);
				therapyPlaces.Add(therapyPlace);
			}

			return therapyPlaces.ToList();
		}

		private static IEnumerable<TherapyPlaceType> AcceptTherapyPlaceTypes(XmlReader reader, int therapyPlaceTypesCount)
		{

			IList<TherapyPlaceType> therapyPlaceTypes = new List<TherapyPlaceType>();

			int i = 0;
			while (i < therapyPlaceTypesCount)
			{
				reader.Read();

				if (reader.NodeType != XmlNodeType.Element || reader.Name != TherapyPlaceType) continue;
				i++;

				var name     = String.Empty;
				var iconType = TherapyPlaceIconType.BedType1;
				var id = new Guid();

				if (reader.HasAttributes)
				{
					while (reader.MoveToNextAttribute())
					{
						if (reader.Name == NameAttribute)     name     = reader.Value;
						if (reader.Name == IconTypeAttribute) iconType = (TherapyPlaceIconType)Enum.Parse(typeof(TherapyPlaceIconType),reader.Value);
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

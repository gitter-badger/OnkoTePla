using System;
using System.IO;
using System.Xml;
using bytePassion.Lib.Types.Communication;
using bytePassion.Lib.Types.Repository;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.LocalSettings
{
	public class LocalSettingsXMLPersistenceService : IPersistenceService<LocalSettingsData>
	{
		private readonly string filename;

		public LocalSettingsXMLPersistenceService(string filename)
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

		private const string XmlRoot        = "localSettings";
		private const string AutoConnection = "autoConnection";
		private const string LastSession    = "lastSession";
		
		
		private const string IsAutoConnectionEnabledAttribute     = "isAutoConnectionEnabled";
		private const string AutoConnectionClientAddressAttribute = "autoConnectionClientAddress";
		private const string AutoConnectionServerAddressAttribute = "autoConnectionServerAddress";

		private const string LastUsedMedicalPracticeIdAttribute   = "lastUsedMedicalPracticeId";
		private const string LastLoggedInUserIdAttribute          = "lastLoggedInUser";

		public void Persist(LocalSettingsData data)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();

			writer.WriteStartElement(XmlRoot);

			WriteAutoconnection(writer, data);
			WriteLastSession(writer, data);

			writer.WriteEndElement();

			writer.WriteEndDocument();
			writer.Close();
		}

		private static void WriteAutoconnection(XmlWriter writer, LocalSettingsData data)
		{
			writer.WriteStartElement(AutoConnection);

			writer.WriteAttributeString(IsAutoConnectionEnabledAttribute,     data.IsAutoConnectionEnabled.ToString());
			writer.WriteAttributeString(AutoConnectionClientAddressAttribute, data.AutoConnectionClientAddress.ToString());
			writer.WriteAttributeString(AutoConnectionServerAddressAttribute, data.AutoConnectionServerAddress.ToString());			

			writer.WriteEndElement();
		}

		private static void WriteLastSession(XmlWriter writer, LocalSettingsData data)
		{
			writer.WriteStartElement(LastSession);
			
			writer.WriteAttributeString(LastUsedMedicalPracticeIdAttribute, data.LastUsedMedicalPracticeId.ToString());
			writer.WriteAttributeString(LastLoggedInUserIdAttribute,        data.LastLoggedInUserId.ToString());

			writer.WriteEndElement();
		}

		public LocalSettingsData Load()
		{
			if (!File.Exists(filename))
				return LocalSettingsData.CreateDefaultSettings();

			bool isAutoConnectionEnabled = false;
			AddressIdentifier autoConnectionClientAddress = new IpV4AddressIdentifier(127,0,0,1);
			AddressIdentifier autoConnectionServerAddress = new IpV4AddressIdentifier(127,0,0,1);
			Guid lastUsedMedicalPracticeId = Guid.Empty;
			Guid lastLoggedInUserId = Guid.Empty;

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element) continue;

					switch (reader.Name)
					{
						case AutoConnection:
						{
							while (reader.MoveToNextAttribute())
							{
								switch (reader.Name)
								{
									case IsAutoConnectionEnabledAttribute:     isAutoConnectionEnabled     = bool.Parse(reader.Value); break;
									case AutoConnectionClientAddressAttribute: autoConnectionClientAddress = AddressIdentifier.GetIpAddressIdentifierFromString(reader.Value); break;
									case AutoConnectionServerAddressAttribute: autoConnectionServerAddress = AddressIdentifier.GetIpAddressIdentifierFromString(reader.Value); break;									
								}
							}
							break;
						}

						case LastSession:
						{
							while (reader.MoveToNextAttribute())
							{
								switch (reader.Name)
								{								
									case LastUsedMedicalPracticeIdAttribute: lastUsedMedicalPracticeId = Guid.Parse(reader.Value); break;
									case LastLoggedInUserIdAttribute:		 lastLoggedInUserId        = Guid.Parse(reader.Value); break;
								}
							}
							break;							
						}
					}

					
					
				}
			}
			reader.Close();

			return new LocalSettingsData(isAutoConnectionEnabled, 
										 autoConnectionClientAddress, 
										 autoConnectionServerAddress,
										 lastUsedMedicalPracticeId,
										 lastLoggedInUserId);
		}
	}
}

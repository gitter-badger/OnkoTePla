using System.IO;
using System.Xml;
using bytePassion.Lib.Types.Repository;

namespace bytePassion.OnkoTePla.Client.DataAndService.LocalSettings
{
	internal class LocalSettingsXMLPersistenceService : IPersistenceService<LocalSettingsData>
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

		private const string XmlRoot = "localSettings";
		private const string Values   = "values";

		private const string IsAutoConnectionEnabledAttribute = "isAutoConnectionEnabled";

		public void Persist(LocalSettingsData data)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();

			writer.WriteStartElement(XmlRoot);

			WriteValues(writer, data);

			writer.WriteEndElement();

			writer.WriteEndDocument();
			writer.Close();
		}

		private static void WriteValues(XmlWriter writer, LocalSettingsData data)
		{
			writer.WriteStartElement(Values);

			writer.WriteAttributeString(IsAutoConnectionEnabledAttribute, data.IsAutoConnectionEnabled.ToString());

			writer.WriteEndElement();
		}

		public LocalSettingsData Load()
		{
			if (!File.Exists(filename))
				return LocalSettingsData.CreateDefaultSettings();

			bool isAutoConnectionEnabled = false;

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != Values) continue;
					if (!reader.HasAttributes) continue;					

					while (reader.MoveToNextAttribute())
					{
						switch (reader.Name)
						{
							case IsAutoConnectionEnabledAttribute: isAutoConnectionEnabled = bool.Parse(reader.Value);break;
						}
					}
					
				}
			}
			reader.Close();

			return new LocalSettingsData(isAutoConnectionEnabled);
		}
	}
}

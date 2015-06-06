using System;
using System.Collections.Generic;
using System.Xml;
using bytePassion.Lib.TimeLib;


namespace bytePassion.OnkoTePla.Client.Core.Repositories.KeyMapper
{
	public class XmlAggregateOfADayKeyMapDataStore : IPersistenceService<IDictionary<Date, Guid>>
	{
		private readonly string filename;

		public XmlAggregateOfADayKeyMapDataStore(string filename)
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

		private const string XmlRoot = "mappings";
		private const string Mapping = "mapping";
		private const string KeyAttribute = "key";
		private const string IdAttribute = "id";

		public void Persist(IDictionary<Date, Guid> data)
		{
			var writer = XmlWriter.Create(filename, WriterSettings);

			writer.WriteStartDocument();

			writer.WriteStartElement(XmlRoot);
						
			foreach (var mapping in data)
			{
				WriteMapping(writer, mapping.Key, mapping.Value);
			}
			
			writer.WriteEndElement();

			writer.WriteEndDocument();
			writer.Close();
		}

		private void WriteMapping(XmlWriter writer, Date key, Guid id)
		{
			writer.WriteStartElement(Mapping);
			writer.WriteAttributeString(KeyAttribute, key.ToString());
			writer.WriteAttributeString(IdAttribute, id.ToString());
			writer.WriteEndElement();
		}

		public IDictionary<Date, Guid> Load()
		{			
			IDictionary<Date, Guid> mappings = new Dictionary<Date, Guid>();

			var reader = XmlReader.Create(filename);

			while (reader.Read())
			{
				if (reader.NodeType != XmlNodeType.Element || reader.Name != XmlRoot) continue;

				while (reader.Read())
				{
					if (reader.NodeType != XmlNodeType.Element || reader.Name != Mapping) continue;
					if (!reader.HasAttributes) continue;

					var key = Date.Dummy;
					var id  = new Guid();

					while (reader.MoveToNextAttribute())
					{
						switch (reader.Name)
						{
							case KeyAttribute: key = Date.Parse(reader.Value); break;
							case IdAttribute:  id  = Guid.Parse(reader.Value); break;
						}
					}
					mappings.Add(key, id);
				}
			}
			reader.Close();

			return mappings;
		}
	}
}

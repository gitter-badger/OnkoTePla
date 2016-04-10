using System;
using System.Text;
using System.Windows.Media;
using bytePassion.OnkoTePla.Contracts.Config;
using bytePassion.OnkoTePla.Contracts.Types;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.Notifications
{
	public class LabelUpdatedNotification : NetworkMessageBase
	{
		public LabelUpdatedNotification (Label label, ConnectionSessionId sessionId)
			: base(NetworkMessageType.LabelUpdatedNotification)
		{
			Label = label;
			SessionId = sessionId;
		}

		public Label Label { get; }
		public ConnectionSessionId SessionId { get; }

		public override string AsString ()
		{
			var sb = new StringBuilder();

			sb.Append(SessionId);

			sb.Append('|');

			sb.Append(Label.Name); sb.Append(';');
			sb.Append(Label.Id);   sb.Append(';');
			sb.Append(Label.Color);

			return sb.ToString();
		}

		public static LabelUpdatedNotification Parse (string s)
		{
			var index = s.IndexOf("|", StringComparison.Ordinal);

			var sessionId = new ConnectionSessionId(Guid.Parse(s.Substring(0, index)));

			var labelData = s.Substring(index + 1, s.Length - index - 1)
				             .Split(';');

			var name   = labelData[0];
			var id     = Guid.Parse(labelData[1]);
			var color  = (Color)ColorConverter.ConvertFromString(labelData[2]);
			
			return new LabelUpdatedNotification(new Label(name, color, id), sessionId);
		}
	}
}
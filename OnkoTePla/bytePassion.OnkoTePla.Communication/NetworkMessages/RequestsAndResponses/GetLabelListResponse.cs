using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Communication.NetworkMessages.RequestsAndResponses
{
	public class GetLabelListResponse : NetworkMessageBase
	{
		public GetLabelListResponse (IReadOnlyList<Label> availableLabels) 
			: base(NetworkMessageType.GetLabelListResponse)
		{
			AvailableLabels = availableLabels;			
		}

		public IReadOnlyList<Label> AvailableLabels { get; } 

		public override string AsString()
		{
			if (AvailableLabels.Count == 0)
				return "";

			var msgBuilder = new StringBuilder();

			foreach (var user in AvailableLabels)
			{
				msgBuilder.Append(user.Name);  msgBuilder.Append(";");
				msgBuilder.Append(user.Id);    msgBuilder.Append(";");
				msgBuilder.Append(user.Color); 								

				msgBuilder.Append("|");
			}

			if (AvailableLabels.Count > 0)
				msgBuilder.Remove(msgBuilder.Length - 1, 1);

			return msgBuilder.ToString();
		}

		public static GetLabelListResponse Parse (string s)
		{
			if (string.IsNullOrWhiteSpace(s))
				return new GetLabelListResponse(new List<Label>());

			var labelList = new List<Label>();
			
			var labelData = s.Split('|')
							 .Select(element => element.Trim())							
							 .Select(userInfo => userInfo.Split(';').ToList());

			foreach (var labelParts in labelData)
			{
				var name  = labelParts[0];
				var id    = Guid.Parse(labelParts[1]);
				var color = (Color)ColorConverter.ConvertFromString(labelParts[2]);
				
				labelList.Add(new Label(name, color, id));				
			}
						
			return new GetLabelListResponse(labelList);
		}
	}
}

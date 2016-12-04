using System;
using System.Collections.Generic;
using System.Linq;
using bytePassion.OnkoTePla.Client.DataAndService.Connection;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.LabelRepository
{
	public class ClientLabelRepository : IClientLabelRepository
	{
		public event Action<Label> NewLabelAvailable;
		public event Action<Label> UpdatedLabelAvailable;

		private readonly IConnectionService connectionService;

		private IList<Label> cachedLabels;  

		public ClientLabelRepository(IConnectionService connectionService)
		{
			this.connectionService = connectionService;

			connectionService.NewLabelAvailable     += OnNewLabelAvailable;
			connectionService.UpdatedLabelAvailable += OnUpdatedLabelAvailable;
		}

		private void OnUpdatedLabelAvailable(Label updatedLabel)
		{
			var oldLabel = cachedLabels.FirstOrDefault(label => label.Id == updatedLabel.Id);

			if (oldLabel == null)
				return;

			cachedLabels.Remove(oldLabel);
			cachedLabels.Add(updatedLabel);

			UpdatedLabelAvailable?.Invoke(updatedLabel);
		}

		private void OnNewLabelAvailable(Label label)
		{
			cachedLabels.Add(label);
			NewLabelAvailable?.Invoke(label);
		}

		public void RequestLabel(Action<Label> labelAvailableCallback, Guid labelId, Action<string> errorCallback)
		{
			if (cachedLabels == null)
			{
				connectionService.RequestLabelList(
					labelList =>
					{
						cachedLabels = labelList.ToList();
						labelAvailableCallback(GetLabel(labelId, errorCallback));
					},
					errorCallback					
				);
			}
			else
			{
				labelAvailableCallback(GetLabel(labelId, errorCallback));
			}
		}

		public void RequestAllLabels(Action<IReadOnlyList<Label>> labelsAvailableCallback, Action<string> errorCallback)
		{
			if (cachedLabels == null)
			{
				connectionService.RequestLabelList(
					labelList =>
					{
						cachedLabels = labelList.ToList();						
						labelsAvailableCallback(cachedLabels.ToList());
					},
					errorCallback
				);
			}
			else
			{
				labelsAvailableCallback(cachedLabels.ToList());
			}
		}

		private Label GetLabel(Guid id, Action<string> errorCallback)
		{
			var resultLabel = cachedLabels.FirstOrDefault(label => label.Id == id);

			if (resultLabel == null)
			{
				errorCallback("label not found");
				return null;
			}

			return resultLabel;
		}
	}
}
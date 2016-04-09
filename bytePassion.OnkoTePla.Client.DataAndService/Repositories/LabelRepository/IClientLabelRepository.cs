using System;
using System.Collections.Generic;
using bytePassion.OnkoTePla.Contracts.Config;

namespace bytePassion.OnkoTePla.Client.DataAndService.Repositories.LabelRepository
{
	public interface IClientLabelRepository
    {
		event Action<Label> NewLabelAvailable;
		event Action<Label> UpdatedLabelAvailable;
		
		void RequestLabel    (Action<Label> labelAvailableCallback, Guid labelId, Action<string> errorCallback);
	    void RequestAllLabels(Action<IReadOnlyList<Label>> labelsAvailableCallback, Action<string> errorCallback);
    }
}

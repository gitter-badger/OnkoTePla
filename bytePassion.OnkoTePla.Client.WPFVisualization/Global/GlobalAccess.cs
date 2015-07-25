using System;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.Global
{
	public static class GlobalAccess
    {
	    private static ViewModelCommunication<ViewModelMessageBase> viewModelCommunication;

	    public static ViewModelCommunication<ViewModelMessageBase> ViewModelCommunication
	    {
		    get { return viewModelCommunication; }
		    set
		    {
				if (viewModelCommunication == null)
					viewModelCommunication = value;
				else				
					throw new InvalidOperationException("viewModelCommunication is only once settable");				
		    }
	    }
	}
}

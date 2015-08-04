using System;
using bytePassion.Lib.Communication.ViewModel;
using bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.GlobalAccess
{
	public static partial class Global
    {
	    private static ViewModelCommunication<ViewModelMessageBase> viewModelCommunication;
	    public  static ViewModelCommunication<ViewModelMessageBase> ViewModelCommunication
	    {
		    get
		    {
				if (viewModelCommunication == null)
					throw new InvalidOperationException("viewModelCommunication is not yet initialized");

			    return viewModelCommunication;
		    }
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

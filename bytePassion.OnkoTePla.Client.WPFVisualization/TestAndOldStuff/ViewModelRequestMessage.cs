using System;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels.Base
{
	public class ViewModelRequestMessage<TResult> : ViewModelMessage
	{
		
		public ViewModelRequestMessage(Action<TResult> resultCallBack)
		{
			ResultCallBack = resultCallBack;
		}

		public Action<TResult> ResultCallBack { get; }
	}
}

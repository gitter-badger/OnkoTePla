using bytePassion.Lib.Communication.ViewModel.Messages;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModelMessages
{
	public class SetVisibility : ViewModelMessage
	{
		public SetVisibility(bool visible)
		{
			Visible = visible;
		}

		public bool Visible { get; }
	}
}

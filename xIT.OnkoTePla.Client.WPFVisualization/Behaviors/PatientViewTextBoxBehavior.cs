using System;
using System.Windows.Controls;
using System.Windows.Interactivity;


namespace xIT.OnkoTePla.Client.WPFVisualization.Behaviors
{
	internal class PatientViewTextBoxBehavior : Behavior<TextBox>
	{
		private const string TextBoxPromt = "Hier tippen um Liste zu filtern";

		protected override void OnAttached()
		{

			AssociatedObject.TextChanged += OnTextChanged;

		}

		void OnTextChanged (object sender, TextChangedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}

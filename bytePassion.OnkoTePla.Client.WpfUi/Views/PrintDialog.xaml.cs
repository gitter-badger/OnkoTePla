namespace bytePassion.OnkoTePla.Client.WpfUi.Views
{
	public partial class PrintDialog
	{
		public PrintDialog ()
		{
			InitializeComponent();
		}

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var pd = new System.Windows.Controls.PrintDialog();
            if(pd.ShowDialog() == true)
            {
                pd.PrintVisual(controlToPrint, "appointments to print");
            }
        }
    }
}

using bytePassion.Lib.FrameworkExtensions;
using System.ComponentModel;


namespace bytePassion.OnkoTePla.Client.WPFVisualization.ViewModels
{

    public abstract class ViewModel : DisposingObject, 
                                      IViewModel                                      
    {
        public abstract event PropertyChangedEventHandler PropertyChanged;
    }
}
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using bytePassion.Lib.FrameworkExtensions;

namespace bytePassion.Lib.WpfUtils.Panels
{
    
    public class DeckPanel : Panel
    {
		
	    public static readonly DependencyProperty SelectedLayerProperty = 
			DependencyProperty.Register("SelectedLayer", 
										typeof (int), 
										typeof (DeckPanel), 
										new PropertyMetadata(default(int)));

	    public int SelectedLayer
	    {
		    get { return (int) GetValue(SelectedLayerProperty); }
		    set { SetValue(SelectedLayerProperty, value); }
	    }        

        #region Overrides

      
        protected override Size MeasureOverride(Size availableSize)
        {
			return availableSize;	        
        }
       
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children == null || Children.Count == 0)            
                return finalSize;
                       
	        Children.Cast<UIElement>()
					.Where(child => child.Visibility != Visibility.Collapsed)
					.Do(child => child.Visibility = Visibility.Collapsed);
		                   
	        var visibleChild = Children[SelectedLayer];

			visibleChild.Visibility = Visibility.Visible;
	        visibleChild.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));

            return finalSize;
        }

        #endregion      
    }
}



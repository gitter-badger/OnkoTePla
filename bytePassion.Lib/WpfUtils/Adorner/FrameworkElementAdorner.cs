using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;


namespace bytePassion.Lib.WpfUtils.Adorner
{
    
    public class FrameworkElementAdorner : System.Windows.Documents.Adorner
    {
        
        private readonly FrameworkElement adornerContent;
	    private readonly FrameworkElement adornedElement;
     
        private readonly AdornerPlacement horizontalAdornerPlacement;
        private readonly AdornerPlacement verticalAdornerPlacement;

       
        private readonly double offsetX;
        private readonly double offsetY;
               
        

        public FrameworkElementAdorner(FrameworkElement adornerContent, FrameworkElement adornedControl,
                                       AdornerPlacement horizontalAdornerPlacement, AdornerPlacement verticalAdornerPlacement,
                                       double offsetX, double offsetY)
            : base(adornedControl)
        {
            this.adornerContent = adornerContent;
	        this.adornedElement = (FrameworkElement)AdornedElement;

            this.horizontalAdornerPlacement = horizontalAdornerPlacement;
            this.verticalAdornerPlacement = verticalAdornerPlacement;
            this.offsetX = offsetX;
            this.offsetY = offsetY;

			adornedControl.SizeChanged += (sender, eventArgs) => InvalidateMeasure();

            AddLogicalChild(adornerContent);
            AddVisualChild(adornerContent);
        }           

        protected override Size MeasureOverride(Size constraint)
        {
            adornerContent.Measure(constraint);
            return adornerContent.DesiredSize;
        }
        
        private double DetermineXPosOfAdornerContent()
        {
            switch (adornerContent.HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                {
                    if (horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -adornerContent.DesiredSize.Width + offsetX;
                    }
                    else
                    {
                        return offsetX;
                    }
                }
                case HorizontalAlignment.Right:
                {
                    if (horizontalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        var adornedWidth = adornedElement.ActualWidth;
                        return adornedWidth + offsetX;
                    }
                    else
                    {
                        var adornerWidth = adornerContent.DesiredSize.Width;
                        var adornedWidth = adornedElement.ActualWidth;
                        var x = adornedWidth - adornerWidth;
                        return x + offsetX;
                    }
                }
                case HorizontalAlignment.Center:
                {
                    var adornerWidth = adornerContent.DesiredSize.Width;
                    var adornedWidth = adornedElement.ActualWidth;
                    var x = (adornedWidth / 2) - (adornerWidth / 2);
                    return x + offsetX;
                }
                case HorizontalAlignment.Stretch:
                {
                    return 0.0;
                }
            }

            return 0.0;
        }
        
        private double DetermineYPosOfAdornerContent()
        {
            switch (adornerContent.VerticalAlignment)
            {
                case VerticalAlignment.Top:
                {
                    if (verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        return -adornerContent.DesiredSize.Height + offsetY;
                    }
                    else
                    {
                        return offsetY;
                    }
                }
                case VerticalAlignment.Bottom:
                {
                    if (verticalAdornerPlacement == AdornerPlacement.Outside)
                    {
                        var adornedHeight = adornedElement.ActualHeight;
                        return adornedHeight + offsetY;
                    }
                    else
                    {
                        var adornerHeight = adornerContent.DesiredSize.Height;
                        var adornedHeight = adornedElement.ActualHeight;
                        var x = adornedHeight - adornerHeight;
                        return x + offsetY;
                    }
                }
                case VerticalAlignment.Center:
                {
                    var adornerHeight = adornerContent.DesiredSize.Height;
                    var adornedHeight = adornedElement.ActualHeight;
                    var x = (adornedHeight / 2) - (adornerHeight / 2);
                    return x + offsetY;
                }
                case VerticalAlignment.Stretch:
                {
                    return 0.0;
                }
            }

            return 0.0;
        }

       
        protected override Size ArrangeOverride(Size finalSize)
        {
            var x =  DetermineXPosOfAdornerContent();            
			var y =  DetermineYPosOfAdornerContent();

			var adornerWidth = adornerContent.HorizontalAlignment == HorizontalAlignment.Stretch 
				? adornedElement.ActualWidth
				: adornerContent.DesiredSize.Width;

			var adornerHeight = adornerContent.VerticalAlignment == VerticalAlignment.Stretch 
				? adornedElement.ActualHeight
				: adornerContent.DesiredSize.Height;

			adornerContent.Arrange(new Rect(x, y, adornerWidth, adornerHeight));

            return finalSize;
        }

        protected override Int32 VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(Int32 index)
        {
            return adornerContent;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
				var list = new ArrayList(VisualChildrenCount)
	                             {
		                             adornerContent
	                             };

	            return list.GetEnumerator();
            }
        }
       
        public void DisconnectAdornerContent()
        {
            RemoveLogicalChild(adornerContent);
            RemoveVisualChild(adornerContent);
        }
                
    }
}
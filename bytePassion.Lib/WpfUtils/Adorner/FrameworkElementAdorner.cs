using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;


namespace bytePassion.Lib.WpfUtils.Adorner
{
	
	public class FrameworkElementAdorner : System.Windows.Documents.Adorner
	{
		
		private readonly FrameworkElement adorner;
		
		private readonly AdornerPlacement horizontalAdornerPlacement = AdornerPlacement.Inside;
		private readonly AdornerPlacement verticalAdornerPlacement   = AdornerPlacement.Inside;
		
		private readonly double offsetX;
		private readonly double offsetY;
		
		public FrameworkElementAdorner (FrameworkElement adorner, FrameworkElement adornedElement)
			: base(adornedElement)
		{
			this.adorner = adorner;

			AddLogicalChild(adorner);
			AddVisualChild(adorner);
		}

		public FrameworkElementAdorner (FrameworkElement adorner, 
										FrameworkElement adornedElement,
									    AdornerPlacement horizontalAdornerPlacement, 
									    AdornerPlacement verticalAdornerPlacement,
									    double offsetX, 
										double offsetY)
			: this(adorner, adornedElement)
		{			
			this.horizontalAdornerPlacement = horizontalAdornerPlacement;
			this.verticalAdornerPlacement = verticalAdornerPlacement;
			this.offsetX = offsetX;
			this.offsetY = offsetY; 

			adornedElement.SizeChanged += (sender, args) => InvalidateMeasure();			
		}
				
		public double PositionX { get; set; } = Double.NaN;
		public double PositionY { get; set; } = Double.NaN;

		protected override Size MeasureOverride (Size constraint)
		{
			adorner.Measure(constraint);
			return adorner.DesiredSize;
		}

		protected override Size ArrangeOverride (Size finalSize)
		{
			double x = PositionX;
			double y = PositionY;

			if (Double.IsNaN(x)) x = DetermineX();			
			if (Double.IsNaN(y)) y = DetermineY();

			double adornerWidth = DetermineWidth();
			double adornerHeight = DetermineHeight();

			adorner.Arrange(new Rect(x, y, adornerWidth, adornerHeight));

			return finalSize;
		}

		private double DetermineX ()
		{
			switch (adorner.HorizontalAlignment)
			{
				case HorizontalAlignment.Left:
				{
					if (horizontalAdornerPlacement == AdornerPlacement.Outside)					
						return -adorner.DesiredSize.Width + offsetX;					
					else					
						return offsetX;					
				}
				case HorizontalAlignment.Right:
				{
					if (horizontalAdornerPlacement == AdornerPlacement.Outside)
					{
						double adornedWidth = AdornedElement.ActualWidth;
						return adornedWidth + offsetX;
					}
					else
					{
						double adornerWidth = adorner.DesiredSize.Width;
						double adornedWidth = AdornedElement.ActualWidth;
						double x = adornedWidth - adornerWidth;
						return x + offsetX;
					}
				}
				case HorizontalAlignment.Center:
				{
					double adornerWidth = adorner.DesiredSize.Width;
					double adornedWidth = AdornedElement.ActualWidth;
					double x = (adornedWidth / 2) - (adornerWidth / 2);
					return x + offsetX;
				}
				case HorizontalAlignment.Stretch:
				{
					return 0.0;
				}
			}
			return 0.0;
		}
		
		private double DetermineY ()
		{
			switch (adorner.VerticalAlignment)
			{
				case VerticalAlignment.Top:
				{
					if (verticalAdornerPlacement == AdornerPlacement.Outside)					
						return -adorner.DesiredSize.Height + offsetY;					
					else					
						return offsetY;
					
				}
				case VerticalAlignment.Bottom:
				{
					if (verticalAdornerPlacement == AdornerPlacement.Outside)
					{
						double adornedHeight = AdornedElement.ActualHeight;
						return adornedHeight + offsetY;
					}
					else
					{
						double adornerHeight = adorner.DesiredSize.Height;
						double adornedHeight = AdornedElement.ActualHeight;
						double x = adornedHeight - adornerHeight;
						return x + offsetY;
					}
				}
				case VerticalAlignment.Center:
				{
					double adornerHeight = adorner.DesiredSize.Height;
					double adornedHeight = AdornedElement.ActualHeight;
					double x = (adornedHeight / 2) - (adornerHeight / 2);
					return x + offsetY;
				}
				case VerticalAlignment.Stretch:
				{
					return 0.0;
				}
			}
			return 0.0;
		}
		
		private double DetermineWidth ()
		{
			if (!Double.IsNaN(PositionX))			
				return adorner.DesiredSize.Width;
			
			switch (adorner.HorizontalAlignment)
			{
				case HorizontalAlignment.Left:    { return adorner.DesiredSize.Width;  }
				case HorizontalAlignment.Right:   { return adorner.DesiredSize.Width;  }
				case HorizontalAlignment.Center:  { return adorner.DesiredSize.Width;  }
				case HorizontalAlignment.Stretch: { return AdornedElement.ActualWidth; }
			}

			return 0.0;
		}
		
		private double DetermineHeight ()
		{
			if (!Double.IsNaN(PositionY))			
				return adorner.DesiredSize.Height;
			
			switch (adorner.VerticalAlignment)
			{
				case VerticalAlignment.Top:     { return adorner.DesiredSize.Height;  }
				case VerticalAlignment.Bottom:  { return adorner.DesiredSize.Height;  }
				case VerticalAlignment.Center:  { return adorner.DesiredSize.Height;  }
				case VerticalAlignment.Stretch: { return AdornedElement.ActualHeight; }
			}
			return 0.0;
		}

		
		protected override Int32 VisualChildrenCount => 1;
		protected override Visual GetVisualChild (Int32 index) => adorner;

		public new FrameworkElement AdornedElement => (FrameworkElement)base.AdornedElement;

		protected override IEnumerator LogicalChildren
		{
			get
			{
				var list = new ArrayList
				           {
					           adorner
				           };
				return list.GetEnumerator();
			}
		}
		
		public void DisconnectChild ()
		{
			RemoveLogicalChild(adorner);
			RemoveVisualChild(adorner);
		}				
	}
}
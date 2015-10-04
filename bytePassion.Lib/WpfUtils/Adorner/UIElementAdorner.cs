using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace bytePassion.Lib.WpfUtils.Adorner
{
    public class UIElementAdorner : System.Windows.Documents.Adorner
    {
        private readonly AdornerLayer layer;

        private ContentPresenter presenter;
        private double _leftOffset;
        private double _topOffset;

        public UIElementAdorner(UIElement adornedElement, DataTemplate adornderContent, object data, AdornerLayer layer) : base(adornedElement)
        {
            this.layer = layer;
            Focusable = false;
            IsHitTestVisible = false;
            presenter = new ContentPresenter() { Content = data, ContentTemplate = adornderContent, Opacity = 0.7};
            layer.Add(this);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            presenter.Measure(constraint);
            return presenter.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            presenter.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return presenter;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void UpdatePosition(double left, double top)
        {
            _leftOffset = left;
            _topOffset = top;
            if (layer!= null)
            {
                layer.Update(this.AdornedElement);
            }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, 0));
            return result;
        }

        public void Destroy()
        {
            layer.Remove(this);
        }

    }
}
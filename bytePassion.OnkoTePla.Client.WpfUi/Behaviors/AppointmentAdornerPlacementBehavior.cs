using bytePassion.Lib.FrameworkExtensions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;


namespace bytePassion.OnkoTePla.Client.WpfUi.Behaviors
{
    public class AppointmentAdornerPlacementBehavior : Behavior<StackPanel>
    {
        public static readonly DependencyProperty ReferenceElementProperty = DependencyProperty.Register(
            "ReferenceElement", typeof (UIElement), typeof (AppointmentAdornerPlacementBehavior), new PropertyMetadata(default(UIElement)));

        private int zindex;

        public UIElement ReferenceElement
        {
            get { return (UIElement) GetValue(ReferenceElementProperty); }
            set { SetValue(ReferenceElementProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.IsVisibleChanged += OnLayoutUpdate;
            zindex = Canvas.GetZIndex(AssociatedObject);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.IsVisibleChanged -= OnLayoutUpdate;
        }

        private void OnLayoutUpdate(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!ReferenceElement.IsUserVisible())
            {
                AssociatedObject.Margin = new Thickness(0, 30, 0, -25);
            }
            else
            {
                AssociatedObject.Margin = new Thickness(0, -25,0, 30); 
            }
        }
    }
}
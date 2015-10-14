using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace bytePassion.Lib.FrameworkExtensions
{
    public static class FrameworkElementExtensions
    {
        public static bool IsUserVisible(this UIElement element)
        {
            if (!element.IsVisible)
                return false;
            var container = FindVisualParent<ScrollViewer>(element);
            if (container == null) throw new ArgumentNullException("container");

            var relativePoint = element.TranslatePoint(new Point(0.0, 0.0), container);

            return relativePoint.Y > 30;
        }

        public static T FindVisualParent<T>(DependencyObject child)
            where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            var parent = parentObject as T;
            return parent ?? FindVisualParent<T>(parentObject);
        }
    }
}
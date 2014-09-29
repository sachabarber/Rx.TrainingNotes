using System.Windows;
using System.Windows.Input;

namespace ReactiveWPF.Common
{
    public class EnterKeyTraversal
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(EnterKeyTraversal), new UIPropertyMetadata(false, IsEnabledChanged));

        static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ue = d as FrameworkElement;
            if (ue == null) return;

            if ((bool)e.NewValue)
            {
                ue.Unloaded += OnUnloaded;
                ue.PreviewKeyDown += OnPreviewKeyDown;
            }
            else
            {
                ue.PreviewKeyDown -= OnPreviewKeyDown;
            }
        }

        static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var ue = e.OriginalSource as FrameworkElement;
            if (ue == null) return;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private static void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var ue = sender as FrameworkElement;
            if (ue == null) return;

            ue.Unloaded -= OnUnloaded;
            ue.PreviewKeyDown -= OnPreviewKeyDown;
        }
    }
}
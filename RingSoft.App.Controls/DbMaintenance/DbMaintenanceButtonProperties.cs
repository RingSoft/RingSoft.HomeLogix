// ReSharper disable once CheckNamespace

using System.Windows;
using System.Windows.Media;

// ReSharper disable once CheckNamespace
namespace RingSoft.App.Controls
{
    public class DbMaintenanceButtonProperties
    {
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.RegisterAttached("Image", typeof(ImageSource), typeof(DbMaintenanceButtonProperties),
                new UIPropertyMetadata((ImageSource)null));
        public static ImageSource GetImage(DependencyObject obj)
        {
            return (ImageSource)obj.GetValue(ImageProperty);
        }

        public static void SetImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ImageProperty, value);
        }

        //public static readonly DependencyProperty ToolTipHeaderProperty =
        //    DependencyProperty.RegisterAttached("ToolTipHeader", typeof(string), typeof(DbMaintenanceButtonProperties),
        //        new UIPropertyMetadata((string)null));

        //public static string GetToolTipHeader(DependencyObject obj)
        //{
        //    return (string)obj.GetValue(ToolTipHeaderProperty);
        //}

        //public static void SetToolTipHeader(DependencyObject obj, string value)
        //{
        //    obj.SetValue(ToolTipHeaderProperty, value);
        //}


    }
}

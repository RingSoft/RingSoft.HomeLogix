﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RingSoft.DataEntryControls.Engine;

// ReSharper disable once CheckNamespace
namespace RingSoft.App.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls.DbMaintenance"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls.DbMaintenance;assembly=RingSoft.App.Controls.DbMaintenance"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:DbMaintenanceButton/>
    ///
    /// </summary>
    [TemplatePart (Name = "Image", Type = typeof(Image))]
    [TemplatePart (Name = "DbMaintenanceToolTip", Type = typeof(DbMaintenanceToolTip))]
    public class DbMaintenanceButton : Button
    {
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.RegisterAttached(nameof(ImageSource), typeof(ImageSource), typeof(DbMaintenanceButton),
                new FrameworkPropertyMetadata(null, ImageSourceChangedCallback));

        public ImageSource ImageSource
        {
            get => (ImageSource) GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        private static void ImageSourceChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dbMaintenanceButton = (DbMaintenanceButton)obj;
            dbMaintenanceButton.SetImage();
        }

        public static readonly DependencyProperty ToolTipHeaderProperty =
            DependencyProperty.RegisterAttached(nameof(ToolTipHeader), typeof(string), typeof(DbMaintenanceButton),
                new FrameworkPropertyMetadata(null, ToolTipHeaderChangedCallback));

        public string ToolTipHeader
        {
            get => (string)GetValue(ToolTipHeaderProperty);
            set => SetValue(ToolTipHeaderProperty, value);
        }

        private static void ToolTipHeaderChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var dbMaintenanceButton = (DbMaintenanceButton)obj;
            dbMaintenanceButton.SetToolTipProperties();
        }

        public Image Image { get; set; }

        public DbMaintenanceToolTip DbMaintenanceToolTip { get; set; }

        static DbMaintenanceButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DbMaintenanceButton), new FrameworkPropertyMetadata(typeof(DbMaintenanceButton)));
        }

        public override void OnApplyTemplate()
        {
            Image = GetTemplateChild(nameof(Image)) as Image;
            DbMaintenanceToolTip = GetTemplateChild(nameof(DbMaintenanceToolTip)) as DbMaintenanceToolTip;

            SetImage();
            SetToolTipProperties();

            base.OnApplyTemplate();
        }

        private void SetImage()
        {
            if (Image != null && ImageSource != null)
                Image.Source = ImageSource;
        }

        private void SetToolTipProperties()
        {
            if (DbMaintenanceToolTip != null && !ToolTipHeader.IsNullOrEmpty())
                DbMaintenanceToolTip.HeaderText = ToolTipHeader;
        }
    }
}

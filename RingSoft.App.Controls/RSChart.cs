using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using ScottPlot;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;
using Google.Protobuf.WellKnownTypes;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using Color = System.Drawing.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Orientation = System.Windows.Controls.Orientation;

namespace RingSoft.App.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:RingSoft.App.Controls;assembly=RingSoft.App.Controls"
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
    ///     <MyNamespace:RSChart/>
    ///
    /// </summary>
    [TemplatePart(Name = "WpfPlot", Type = typeof(WpfPlot))]
    [TemplatePart(Name = "TitleLabel", Type = typeof(Label))]
    [TemplatePart(Name = "LegendPanel", Type = typeof(StackPanel))]
    public class RSChart : Control
    {
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.RegisterAttached(nameof(Items), typeof(ChartData), typeof(RSChart),
                new FrameworkPropertyMetadata(null, ItemsChangedCallback));

        public ChartData Items
        {
            get => (ChartData)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        private static void ItemsChangedCallback(DependencyObject obj,
            DependencyPropertyChangedEventArgs args)
        {
            var control = (RSChart)obj;
            if (control.WpfPlot != null)
                control.SetChartData(control.Items);
        }

        public WpfPlot WpfPlot { get; set; }
        public StackPanel LegendPanel { get; set; }
        public Label TitleLabel { get; set; }

        static RSChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RSChart), new FrameworkPropertyMetadata(typeof(RSChart)));
        }

        public void SetChartData(ChartData items)
        {
            if (items == null)
                return;

            var values = new List<double>();
            var captions = new List<string>();
            var colors = new List<Color>();
            var total = 0.0;

            var index = 1;
            foreach (var chartDataItem in items.Items)
            {
                values.Add(chartDataItem.Value);
                captions.Add(chartDataItem.Name);

                MakeColors(index, colors);

                index++;
                if (index % 10 == 0)
                    index = 1;
            }

            if (WpfPlot != null)
            {
                WpfPlot.Refresh();

                if (LegendPanel != null)
                {
                    LegendPanel.Children.Clear();
                    var groupBox = new GroupBox();
                    groupBox.HorizontalAlignment = HorizontalAlignment.Right;
                    LegendPanel.Height = WpfPlot.ActualHeight;
                    groupBox.UpdateLayout();
                    
                    var groupPanel = new StackPanel();
                    groupPanel.Orientation = Orientation.Vertical;

                    index = 0;
                    total = values.Sum();
                    List<double> newValues = new List<double>();
                    List<Color> newColors = new List<Color>();

                    var everythingElse = 0.0;
                    var text = string.Empty;
                    var height = 20;
                    foreach (var item in items.Items)
                    {
                        
                        var rectangle = new Rectangle();
                        rectangle.Height = 10;
                        rectangle.Width = 50;

                        var value = 0.0;

                        if (height > WpfPlot.ActualHeight - 20)
                        {
                            everythingElse += item.Value;
                            if (item == Items.Items[^1])
                            {
                                text = "Everything Else";
                                value = everythingElse;
                                newValues.Add(value);
                                newColors.Add(colors[index]);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            text = item.Name;
                            value = item.Value;
                            newValues.Add(value);
                            newColors.Add(colors[index]);
                        }
                        var itemPanel = new StackPanel();
                        itemPanel.Orientation = Orientation.Horizontal;

                        rectangle.Fill = new SolidColorBrush(newColors[^1].GetMediaColor());
                        
                        itemPanel.Children.Add(rectangle);

                        var textBlock = new TextBlock();
                        textBlock.Text = text;
                        textBlock.Margin = new Thickness(5, 0, 0, 0);
                        itemPanel.Children.Add(textBlock);

                        var percentValue = value / total;
                        var percentTextBlock = new TextBlock();
                        percentTextBlock.Margin = new Thickness(5, 0, 0, 0);

                        percentTextBlock.Text = GblMethods.FormatValue(FieldDataTypes.Decimal, percentValue.ToString(), 
                            GblMethods.GetPercentFormat(2));

                        itemPanel.Children.Add(percentTextBlock);

                        groupPanel.Children.Add(itemPanel);
                        groupPanel.UpdateLayout();
                        height += 20;
                        index++;
                    }

                    if (Items.Items.Any())
                    {
                        WpfPlot.Plot.Clear();
                        var pie = WpfPlot.Plot.AddPie(newValues.ToArray());

                        pie.ShowValues = false;
                        pie.SliceFillColors = newColors.ToArray();

                        WpfPlot.Plot.Grid(false);
                        WpfPlot.Plot.XTicks();
                        WpfPlot.Plot.YTicks();
                        WpfPlot.Plot.Style(dataBackground: Color.Transparent, figureBackground: Color.Transparent);

                        groupBox.Content = groupPanel;
                        groupBox.Header = "Legend";
                        groupBox.UpdateLayout();
                        WpfPlot.UpdateLayout();

                        LegendPanel.Children.Add(groupBox);

                        groupBox.Height = WpfPlot.ActualHeight - 20;
                        groupBox.UpdateLayout();
                    }
                }
                LegendPanel.UpdateLayout();
                TitleLabel.Content = Items.Title;
            }
        }

        private static void MakeColors(int index, List<Color> colors)
        {
            if (index == 1)
            {
                colors.Add(Color.Green);
            }
            else if (index == 2)
            {
                colors.Add(Color.Red);
            }
            else if (index == 3)
            {
                colors.Add(Color.Blue);
            }
            else if (index == 4)
            {
                colors.Add(Color.Yellow);
            }
            else if (index == 5)
            {
                colors.Add(Color.DarkGray);
            }
            else if (index == 6)
            {
                colors.Add(Color.LightGreen);
            }
            else if (index == 7)
            {
                colors.Add(Color.LightPink);
            }
            else if (index == 8)
            {
                colors.Add(Color.LightBlue);
            }
            else if (index == 9)
            {
                colors.Add(Color.LightYellow);
            }
            else if (index == 10)
            {
                colors.Add(Color.LightGray);
            }
        }

        public override void OnApplyTemplate()
        {
            WpfPlot = GetTemplateChild("WpfPlot") as WpfPlot;
            LegendPanel = GetTemplateChild("LegendPanel") as StackPanel;
            TitleLabel = GetTemplateChild("TitleLabel") as Label;

            SetChartData(Items);

            base.OnApplyTemplate();
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using RingSoft.App.Library;
using RingSoft.DataEntryControls.WPF;

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
    ///     <MyNamespace:SqlServerLogin/>
    ///
    /// </summary>
    public class SqlServerLogin : Control, ISqlServerView
    {
        public string Password
        {
            get => PasswordBox.Password;
            set => PasswordBox.Password = value;
        }

        public Border Border { get; set; }

        public  SqlServerLoginViewModel ViewModel { get; private set; }

        public StringEditControl ServerTextBox { get; set; }

        public TextComboBoxControl DatabaseComboBox { get; set; }

        public PasswordBox PasswordBox { get; set; }

        static SqlServerLogin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SqlServerLogin), new FrameworkPropertyMetadata(typeof(SqlServerLogin)));
        }

        public SqlServerLogin()
        {
            Loaded += (sender, args) =>
            {
                ViewModel.OnViewLoaded(this);
                DatabaseComboBox.GotFocus += DatabaseComboBox_GotFocus;
            };
        }

        private void DatabaseComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ViewModel.DatabaseGotFocus();
            if (ViewModel.DatabasesList != null)
            {
                var text = DatabaseComboBox.Text;
                DatabaseComboBox.Items.Clear();
                DatabaseComboBox.Text = text;
                var itemIndex = 0;
                foreach (var databaseName in ViewModel.DatabasesList)
                {
                    DatabaseComboBox.Items.Add(databaseName);
                    if (databaseName == DatabaseComboBox.Text)
                        DatabaseComboBox.SelectedIndex = itemIndex;

                    itemIndex++;
                }

            }

        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            ViewModel = Border.TryFindResource("SqlServerLoginViewModel") as SqlServerLoginViewModel;

            ServerTextBox = GetTemplateChild(nameof(ServerTextBox)) as StringEditControl;
            DatabaseComboBox = GetTemplateChild(nameof(DatabaseComboBox)) as TextComboBoxControl;
            PasswordBox = GetTemplateChild(nameof(PasswordBox)) as PasswordBox;

            base.OnApplyTemplate();
        }
    }
}

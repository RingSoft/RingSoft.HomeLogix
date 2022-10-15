using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using RingSoft.App.Library;

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
    ///     <MyNamespace:SqliteLogin/>
    ///
    /// </summary>
    public class SqliteLogin : Control, ISqliteLoginView
    {
        public Border Border { get; set; }
        public SqliteLoginViewModel ViewModel { get; set; }

        static SqliteLogin()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SqliteLogin), new FrameworkPropertyMetadata(typeof(SqliteLogin)));
        }

        public SqliteLogin()
        {
            Loaded += (sender, args) => Initialize();
        }

        private void Initialize()
        {
            ViewModel = Border.TryFindResource("SqliteLoginViewModel") as SqliteLoginViewModel;
            ViewModel.Initialize(this);
        }

        public override void OnApplyTemplate()
        {
            Border = GetTemplateChild(nameof(Border)) as Border;
            base.OnApplyTemplate();
        }

        public string ShowFileDialog()
        {
            var fileName = Path.GetFileName(ViewModel.FilenamePath);
            var directory = string.Empty;
            if (ViewModel.FilenamePath != null) directory = new FileInfo(ViewModel.FilenamePath).DirectoryName;
            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileName ?? string.Empty,
                InitialDirectory = directory ?? string.Empty,
                DefaultExt = "sqlite",
                Filter = $"{ViewModel.ModelName} SQLite Files(*.sqlite)|*.sqlite"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }

            return string.Empty;

        }
    }
}

using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.HomeLogix.DataAccess.Model;
using RingSoft.HomeLogix.Library.ViewModels.Budget;

namespace RingSoft.HomeLogix.Budget
{
    /// <summary>
    /// Interaction logic for BudgetExpenseWindow.xaml
    /// </summary>
    public partial class BudgetExpenseWindow : IBudgetExpenseView
    {
        public BudgetExpenseWindow()
        {
            InitializeComponent();

            Loaded += (sender, args) => ViewModel.OnViewLoaded(this, new BudgetItem());
        }

        public void SetViewType(RecurringViewTypes viewType)
        {
            
        }

        public void OnValidationFail(FieldDefinition failedFieldDefinition)
        {
            
        }

        public void CloseWindow()
        {
            Close();
        }
    }
}

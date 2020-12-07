using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
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

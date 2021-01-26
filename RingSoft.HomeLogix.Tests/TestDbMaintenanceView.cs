using System;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.HomeLogix.Tests
{
    public class TestDbMaintenanceView : IDbMaintenanceView
    {
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Console.WriteLine($"Validation Fail: {text}");
        }

        public void ResetViewForNewRecord()
        {
            Console.WriteLine(nameof(ResetViewForNewRecord));
        }

        public void OnRecordSelected()
        {
            Console.WriteLine(nameof(OnRecordSelected));
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            
        }

        public void CloseWindow()
        {
            
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption)
        {
            Console.WriteLine($"Show YesNoCancel Message:  {text}");
            return MessageButtons.Yes;
        }

        public bool ShowYesNoMessage(string text, string caption)
        {
            Console.WriteLine($"ShowYesNoMessage: {text}");
            return true;
        }

        public void ShowRecordSavedMessage()
        {
            Console.WriteLine("Record Saved");
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            Console.WriteLine(nameof(SetReadOnlyMode));
        }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;
    }
}

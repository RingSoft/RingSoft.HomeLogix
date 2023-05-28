using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Media;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Lookup;
using RingSoft.DbLookup.ModelDefinition.FieldDefinitions;
using RingSoft.DbMaintenance;

namespace RingSoft.HomeLogix.Tests
{
    public class TestDbMaintenanceView : IDbMaintenanceView
    {
        private string _ownerName;

        public TestDbMaintenanceView(string ownerName)
        {
            _ownerName = ownerName;
        }
        public void OnValidationFail(FieldDefinition fieldDefinition, string text, string caption)
        {
            Debug.WriteLine($"{_ownerName} Validation Fail: {text}");
            SystemSounds.Exclamation.Play();
        }

        public void HandleAutoFillValFail(DbAutoFillMap autoFillMap)
        {
            
        }

        public void ResetViewForNewRecord()
        {
        }

        public void OnRecordSelected()
        {
        }

        public void ShowFindLookupWindow(LookupDefinitionBase lookupDefinition, bool allowAdd, bool allowView, string initialSearchFor,
            PrimaryKeyValue initialSearchForPrimaryKey)
        {
            
        }

        public void CloseWindow()
        {
            
        }

        public MessageButtons ShowYesNoCancelMessage(string text, string caption, bool playSound = false)
        {
            Debug.WriteLine($"{_ownerName} Show YesNoCancel Message:  {text}");
            return MessageButtons.Yes;
        }

        public bool ShowYesNoMessage(string text, string caption, bool playSound = false)
        {
            Debug.WriteLine($"{_ownerName} ShowYesNoMessage: {text}");
            return true;
        }

        public void ShowRecordSavedMessage()
        {
        }

        public void SetReadOnlyMode(bool readOnlyValue)
        {
            Debug.WriteLine($"{_ownerName} {nameof(SetReadOnlyMode)}");
        }

        public List<DbAutoFillMap> GetAutoFills()
        {
            return null;
        }

        public event EventHandler<LookupSelectArgs> LookupFormReturn;
    }
}

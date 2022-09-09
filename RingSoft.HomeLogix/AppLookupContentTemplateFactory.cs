using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using RingSoft.DataEntryControls.WPF;
using RingSoft.DbLookup;
using RingSoft.DbLookup.Controls.WPF;
using RingSoft.HomeLogix.DataAccess;
using RingSoft.HomeLogix.Library;

namespace RingSoft.HomeLogix
{
    public class AppLookupContentTemplateFactory : LookupControlContentTemplateFactory
    {
        private Application _application;

        public AppLookupContentTemplateFactory(Application application)
        {
            _application = application;
        }

        public override DataEntryCustomContentTemplate GetContentTemplate(int contentTemplateId)
        {
            if (contentTemplateId == HomeLogixLookupContext.RegisterTypeCustomContentId)
                return _application.Resources["RegisterLineType"] as DataEntryCustomContentTemplate;
            else if (contentTemplateId == HomeLogixLookupContext.BudgetItemTypeContentId)
            {
                var result = _application.Resources["BudgetItemType"] as DataEntryCustomContentTemplate;
                return result;
            }

            return base.GetContentTemplate(contentTemplateId);
        }

        public override Image GetImageForAlertLevel(AlertLevels alertLevel)
        {
            Image result = null;
            switch (alertLevel)
            {
                case AlertLevels.Green:
                    result = _application.Resources["GreenAlertImage"] as Image;
                    return result;
                case AlertLevels.Yellow:
                    result = _application.Resources["YellowAlertImage"] as Image;
                    return result;
                case AlertLevels.Red:
                    result = _application.Resources["RedAlertImage"] as Image;
                    return result;
                default:
                    throw new ArgumentOutOfRangeException(nameof(alertLevel), alertLevel, null);
            }
            return base.GetImageForAlertLevel(alertLevel);
        }
    }
}

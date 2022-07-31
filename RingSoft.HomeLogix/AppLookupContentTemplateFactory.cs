using System.Windows;
using RingSoft.DataEntryControls.WPF;
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

            return base.GetContentTemplate(contentTemplateId);
        }
    }
}

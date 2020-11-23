using System.Threading;
using RingSoft.App.Library;

namespace RingSoft.HomeLogix.Library
{
    public class HomeLogixGlobals : RingSoftAppGlobals
    {
        public override bool InitGlobals()
        {
            Thread.Sleep(5000);
            return true;
        }
    }
}

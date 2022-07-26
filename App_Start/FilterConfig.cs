using System.Web;
using System.Web.Mvc;

namespace MITT_HIMAWAN_SUTANTO
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}

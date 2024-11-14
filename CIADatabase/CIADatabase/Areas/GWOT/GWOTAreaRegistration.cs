using System.Web.Mvc;

namespace CIADatabase.Areas.GWOT
{
    public class GWOTAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GWOT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GWOT_default",
                "GWOT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
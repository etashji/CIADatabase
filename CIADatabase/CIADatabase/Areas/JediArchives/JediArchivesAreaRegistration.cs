using System.Web.Mvc;

namespace CIADatabase.Areas.JediArchives
{
    public class JediArchivesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "JediArchives";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "JediArchives_default",
                "JediArchives/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
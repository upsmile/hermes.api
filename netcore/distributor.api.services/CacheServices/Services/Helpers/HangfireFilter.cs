using Hangfire.Dashboard;

namespace CacheServices.Services.Helpers
{
    
    public class HangfireFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
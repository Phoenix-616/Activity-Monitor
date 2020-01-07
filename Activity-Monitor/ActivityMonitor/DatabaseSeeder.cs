using ActivityMonitor.Database;
using System.Threading.Tasks;

namespace ActivityMonitor
{
    public abstract class DatabaseSeeder
    {
        ActivityContext context;
        public DatabaseSeeder(ActivityContext context)
        {
            this.context = context;
        }
        public abstract Task Seed();
    }
}
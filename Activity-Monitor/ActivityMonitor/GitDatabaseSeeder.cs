using System;
using System.Threading.Tasks;
using ActivityMonitor.Database;

namespace ActivityMonitor
{
    class GitDatabaseSeeder : DatabaseSeeder
    {
        public GitDatabaseSeeder(ActivityContext context, string login, string password) : base(context)
        {
        }

        public override Task Seed()
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Threading.Tasks;
using ActivityMonitor.Database;
using ActivityMonitor.GitHubInteraction;

namespace ActivityMonitor
{
    class GitDatabaseSeeder : DatabaseSeeder
    {
        private Crawler crawler;
        RepositoryAttribute[] attributes;
        public GitDatabaseSeeder(ActivityContext context,
                                 string login,
                                 string password,
                                 RepositoryAttribute[] attributes) : base(context)
        {
            crawler = new Crawler(login, password);
            this.attributes = attributes;
        }

        public override Task Seed()
        {
            throw new NotImplementedException();
        }
    }
}

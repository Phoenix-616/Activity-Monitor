using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActivityMonitor.Database;
using ActivityMonitor.Database.Models;
using ActivityMonitor.GitHubInteraction;

namespace ActivityMonitor
{
    class GitDatabaseSeeder : DatabaseSeeder
    {
        private Crawler crawler;
        RepositoryAttribute[] repositories;
        public GitDatabaseSeeder(ActivityContext context,
                                 string login,
                                 string password,
                                 RepositoryAttribute[] repositories) : base(context)
        {
            crawler = new Crawler(login, password);
            this.repositories = repositories;
        }

        public override async Task Seed()
        {
            foreach (var repo in repositories)
            {
                var owner = repo.Owner;
                var name = repo.Name;
                var datas = await crawler.DataGather(owner, name);
                
                /*var devKeys = new List<int>();
                var repKeys = new List<int>();
                var commKeys = new List<int>();
                var fileKeys = new List<int>();*/
                foreach (var data in datas) 
                {
                }
            }

            context.SaveChanges();
        }
    }
}

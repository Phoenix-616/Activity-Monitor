using System;
using System.Collections.Generic;
using System.Linq;
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

        public class Pair
        {
            public int First { get; set; }
            public int Second { get; set; }
        }

        public override async Task Seed()
        {
            foreach (var repo in repositories)
            {
                var owner = repo.Owner;
                var name = repo.Name;
                var datas = await crawler.DataGather(owner, name);

                var repKeys = new List<int>();
                var devpKeys = new List<int>();
                // f - rep, s - dev
                var devRepKeys = new List<Pair>();
                /*var fileKeys = new List<int>();
                var commKeys = new List<int>();*/
                foreach (var data in datas) 
                {
                    await AddRepository(data, repKeys);
                    await AddDeveloper(data, devpKeys);
                    await AddRepAndDev(data, devpKeys, repKeys, devRepKeys);
                    /*AddFiles(data);
                    AddCommits(data);*/
                }
            }
            
            await context.SaveChangesAsync();
        }

        private void AddCommits(Crawler.Data data)
        {
            if (!context.Commits.Any(x => x.Id == data.CommitID))
            {
                context.Commits.Add(new Commit
                {
                    Id = data.CommitID,
                    AuthorId = data.DevID,
                    RepositoryId = data.RepID,
                    CreatedAt = data.CreatedAt,
                    Created = data.Additions,
                    Deleted = data.Deletions
                });
            }
            foreach (var file in data.NameAndSha)
            {
                if (!context.CommitFiles.Any(x => x.CommitId == data.CommitID &&
                 x.FileId == file.Id))
                {
                    context.CommitFiles.Add(new Database.Models.Git.ActivityMonitor.Database.Models.CommitFile
                    {
                        CommitId = data.CommitID,
                        FileId = file.Id
                    });
                }
            }
        }

        private void AddFiles(Crawler.Data data)
        {
            foreach(var file in data.NameAndSha)
            {
                var fileId = file.Id;
                if (!context.Files.Any(x => x.Id == fileId))
                {
                    context.Files.Add(new File
                    {
                        Id = fileId,
                        Name = file.Name,
                        RepositoryId = data.RepID
                });
                }
            }
        }

        private async Task AddRepAndDev(Crawler.Data data,
                                  List<int> devpKeys,
                                  List<int> repKeys,
                                  List<Pair> devRepKeys)
        {
            if (repKeys.Contains(data.RepID) && devpKeys.Contains(data.DevID) &&
                !devRepKeys.Any(x => x.First == data.RepID && x.Second == data.DevID) &&
                !(context.DeveloperRepositories.Any(x => x.RepositoryId == data.RepID &&
                    x.DeveloperId == data.DevID)))
            {
                devRepKeys.Add(new Pair { First = data.RepID, Second = data.DevID });
                await context.DeveloperRepositories.AddAsync(new DeveloperRepository
                {
                    DeveloperId = data.DevID,
                    RepositoryId = data.RepID
                });
            }
        }
        private async Task AddDeveloper(Crawler.Data data,
                                  List<int> devKeys)
        {
            var devId = data.DevID;
            if (!devKeys.Contains(devId))
            {
                devKeys.Add(devId);
                var devs = context.Developers.Where(x => x.Email == data.CommitAuthorEmail).ToArray();
                if (devs.Count() == 0)
                {
                    await context.Developers.AddAsync(new Developer
                    {
                        Id = devId,
                        FirstName = data.CommitAuthorFirstName,
                        LastName = data.CommitAuthorLastName,
                        Login = data.CommitAuthorLogin,
                        Email = data.CommitAuthorEmail
                    });
                }
            }
        }

        private async Task AddRepository(Crawler.Data data,
                                   List<int> repKeys)
        {
            var repos = context.Repositories.Where(x => x.Name == data.RepositoryName).ToArray();
            var repId = data.RepID;

            if (!repKeys.Contains(repId)) 
            {
                repKeys.Add(repId);
                if (repos.Length == 0) 
                {
                    await context.Repositories.AddAsync(new Repository
                    {
                        Id = repId,
                        Name = data.RepositoryName
                    });
                }
            }
        }
    }
}

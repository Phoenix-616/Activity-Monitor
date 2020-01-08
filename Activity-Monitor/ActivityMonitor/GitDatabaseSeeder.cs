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

        public override async Task Seed()
        {
            foreach (var repo in repositories)
            {
                var owner = repo.Owner;
                var name = repo.Name;
                var datas = await crawler.DataGather(owner, name);

                foreach (var data in datas) 
                {
                    AddRepository(data);
                    AddDeveloper(data);
                    AddFiles(data);
                    AddCommits(data);
                }
            }
            
            context.SaveChanges();
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

        private void AddDeveloper(Crawler.Data data)
        {
            var devId = data.DevID;
            if (!context.Developers.Any(x => x.Id == devId))
            {
                context.Developers.Add(new Developer
                {
                    Id = devId,
                    FirstName = data.CommitAuthorFirstName,
                    LastName = data.CommitAuthorLastName,
                    Login = data.CommitAuthorLogin,
                    Email = data.CommitAuthorEmail
                });
                if (!(context.DeveloperRepositories.Any(x => x.RepositoryId == data.RepID &&
                     x.DeveloperId == data.DevID)))
                {
                    context.DeveloperRepositories.Add(new DeveloperRepository
                    {
                        DeveloperId = data.DevID,
                        RepositoryId = data.RepID
                    });
                }
            }
        }

        private void AddRepository(Crawler.Data data)
        {
            var repId = data.RepID;
            if (!context.Repositories.Any(x => x.Id == repId))
            {
                context.Repositories.Add(new Repository
                {
                    Id = repId,
                    Name = data.RepositoryName
                });
                if (context.Developers.Any(x => x.Id == data.DevID) && 
                    !(context.DeveloperRepositories.Any(x => x.RepositoryId == data.RepID && 
                    x.DeveloperId == data.DevID)))
                {
                    context.DeveloperRepositories.Add(new DeveloperRepository
                    {
                        DeveloperId = data.DevID,
                        RepositoryId = data.RepID
                    });
                }
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityMonitor.Database;
using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
using  ActivityMonitor.GitHubInteraction;
using static ActivityMonitor.GitHubInteraction.Crawler;

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
                var datas = await crawler.Get(owner, name);

                await ReposSeed(datas.repositories);
                await DevsSeed(datas.developers);
                await RepDevsSeed(datas.developerRepositories);
                await FilesSeed(datas.files);
                await CommitsSeed(datas.commits);
                await CommitFilesSeed(datas.commitFiles);
            }
        }

        private async Task CommitFilesSeed(List<CommitFileInfo> commitFiles)
        {
            foreach (var commFil in commitFiles)
            {
                var commIdFromContext = context.Commits.Where(x => x.GitId == commFil.Commit.GitId)
                    .ToArray()[0].Id;
                var idOfFileRepos = context.Repositories.Where(x => x.Name == commFil.File.RepName).ToArray()[0].Id;
                var fileIdFromContext = context.Files.Where(x => x.Name == commFil.File.Name &&
                x.RepositoryId == idOfFileRepos).ToArray()[0].Id;
                if (!context.CommitFiles.Any(x => x.FileId == fileIdFromContext && x.CommitId == commIdFromContext))
                {
                    await context.CommitFiles.AddAsync(new CommitFile 
                    { 
                        CommitId = commIdFromContext,
                        FileId = fileIdFromContext
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task CommitsSeed(List<CommitInfo> commits)
        {
            foreach (var comm in commits)
            {
                var reposByName = context.Repositories.Where(x => x.Name == comm.RepositoryName).ToArray();
                var devsByEmail = context.Developers.Where(x => x.Email == comm.AuthorEmail).ToArray();
                var repId = reposByName[0].Id;
                var devId = devsByEmail[0].Id;
                
                if (!context.Commits.Any(x => x.Id == comm.Id || x.GitId == comm.GitId))
                {
                    await context.Commits.AddAsync(new Commit 
                    { 
                        Id = comm.Id,
                        GitId = comm.GitId,
                        AuthorId = devId,
                        RepositoryId = repId,
                        CreatedAt = comm.CreatedAt,
                        Created = comm.Created,
                        Deleted = comm.Deleted
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task FilesSeed(List<FileInfo> files)
        {
            foreach (var file in files)
            {
                var repsByNameFromContext = context.Repositories.Where(x => x.Name == file.RepName).ToArray();
                var repOfFileFromContetx = repsByNameFromContext.Length == 0 ? null : repsByNameFromContext[0];
                var repIdOfFile = repOfFileFromContetx == null ? file.RepId : repOfFileFromContetx.Id;   
                if(!context.Files.Any(x => x.Id == file.Id || (x.Name == file.Name && x.RepositoryId == repIdOfFile)))
                {
                    await context.Files.AddAsync(new File 
                    {
                        Id = file.Id, Name = file.Name, RepositoryId = repIdOfFile
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task RepDevsSeed(List<DeveloperRepository> developerRepositories)
        {
            foreach (var devRep in developerRepositories)
            {
                var repName = devRep.Repository.Name;
                var devEmail = devRep.Developer.Email;
                var repsByName = context.Repositories.Where(x => x.Name == repName).ToArray();
                var devsByEmail = context.Developers.Where(x => x.Email == devEmail).ToArray();
                var repIdToAdd = repsByName.Length == 0 ? devRep.RepositoryId : repsByName[0].Id;
                var devIdToAdd = devsByEmail.Length == 0 ? devRep.DeveloperId : devsByEmail[0].Id;

                if (!context.DeveloperRepositories.Any(x => x.DeveloperId == devIdToAdd &&
                x.RepositoryId == repIdToAdd))
                {
                    await context.DeveloperRepositories.AddAsync(new DeveloperRepository 
                    {
                        DeveloperId = devIdToAdd, RepositoryId = repIdToAdd
                    });
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task DevsSeed(List<Developer> developers)
        {
            foreach (var dev in developers)
            {
                if (!context.Developers.Any(x => x.Email == dev.Email || x.Id == dev.Id))
                {
                    await context.Developers.AddAsync(dev);
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task ReposSeed(List<Repository> repositories)
        {
            foreach(var rep in repositories)
            {
                if(!context.Repositories.Any(x => x.Name == rep.Name || x.Id == rep.Id))
                {
                    await context.Repositories.AddAsync(rep);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActivityMonitor.Database;
using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
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
                var datas = await crawler.Get(owner, name);

                await ReposSeed(datas.repositories);
                await DevsSeed(datas.developers);
                await RepDevsSeed(datas.developerRepositories);
                //await FilesSeed(datas.files);
                //await CommitsSeed(datas.commits);
                //await CommitFilesSeed(datas.commitFiles);
            }
        }

        private async Task CommitFilesSeed(List<CommitFile> commitFiles)
        {
            foreach (var commFil in commitFiles)
            {
                if (!context.CommitFiles.Any(x => x.FileId == commFil.FileId && x.CommitId == commFil.FileId))
                {
                    await context.CommitFiles.AddAsync(commFil);
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task CommitsSeed(List<Commit> commits)
        {
            foreach (var comm in commits)
            {
                if (!context.Commits.Any(x => x.Id == comm.Id && x.GitId == comm.GitId))
                {
                    await context.Commits.AddAsync(comm);
                }
            }
            await context.SaveChangesAsync();
        }

        private async Task FilesSeed(List<File> files)
        {
            foreach (var file in files)
            {
                if (!context.Files.Any(x => x.Id == file.Id || 
                (file.Name == x.Name && 
                context.Repositories.Where(z => z.Id == file.Id).ToArray()[0].Name == 
                context.Repositories.Where(y => y.Id == x.RepositoryId).ToArray()[0].Name)
                ))
                {
                    await context.Files.AddAsync(file);
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

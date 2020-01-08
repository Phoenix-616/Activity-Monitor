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
                if (!context.Files.Any(x => x.Id == file.Id))
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
                if (!context.DeveloperRepositories.Any(x => x.Developer.Email == devRep.Developer.Email &&
                x.Repository.Name == devRep.Repository.Name))
                {
                    await context.DeveloperRepositories.AddAsync(devRep);
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

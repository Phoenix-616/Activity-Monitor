using Octokit;
using System.Threading.Tasks;
using System.Collections.Generic;
using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
using System.Linq;
using System;

namespace ActivityMonitor.GitHubInteraction
{
    class Crawler
    {
        public readonly string NameOfApp = "Activity-Monitor";
        private readonly GitHubClient client;       

        public Crawler(string login, string password)
        {
            client = new GitHubClient(new ProductHeaderValue("Activity-Monitor"));
            var basicAuth = new Credentials(login, password);
            client.Credentials = basicAuth;
        }

        public class Data
        {
            public string commitSha { get; set; }
            public string commitAuthorName { get; set; }
            public string commitAuthorLogin { get;set }
            public string commitAuthorEmail { get; set; }
            public DateTimeOffset createdAt { get; set; }
            public string repositoryName { get; set; }
            public IEnumerable<string> filesNames { get; set; }
            public int additions { get; set; }
            public int deletions { get; set; }
        }

        private async Task<Data> GetData(GitHubCommit commit, string owner, string name)
        {
            var data = new Data
            {
                commitSha = commit.Sha,
                commitAuthorName = commit.Commit.Author.Name,
                commitAuthorLogin = commit.Author.Login,
                commitAuthorEmail = commit.Commit.Author.Email,
                createdAt = commit.Commit.Author.Date,
                repositoryName = owner + "/" + name,
            };
            var currentCommit = await client.Repository.Commit.Get(owner, name, data.commitSha);
            var filesNames = currentCommit.Files.Select(x => x.Filename);
            data.filesNames = filesNames;

            int additions = 0;
            int deletions = 0;

            var additionsArrayByCommit = currentCommit.Files.Select(x => x.Additions);
            var deletionsArrayByCommit = currentCommit.Files.Select(x => x.Deletions);
            foreach (var a in additionsArrayByCommit)
            {
                additions += a;
            }
            foreach (var d in deletionsArrayByCommit)
            {
                deletions += d;
            }

            data.additions = additions;
            data.deletions = deletions;
            return data;
        }

        public async Task<Models> Gathering(RepositoryAttribute [] attributes)
        {
            var models = new Models();
            foreach (var attr in attributes)
            {
                var owner = attr.owner;
                var name = attr.name;
                var commits = await client.Repository.Commit.GetAll(owner, name);
                foreach(var commit in commits)
                {
                    var data = await GetData(commit, owner, name);
                    /*createRepository(data, models);
                    createDeveloper(data, models);
                    createDeveloperRepository(data, models);
                    createFile(data, models);
                    createCommit(data, models);
                    createCommitFile(data, models);*/
                }
            }
            return models;
        }
    }
}


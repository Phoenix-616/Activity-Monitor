using Octokit;
using System.Threading.Tasks;
using ActivityMonitor.Database.Models.Git;
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

        private List<Repository> repositories = new List<Repository>();
        private List<Developer> developers = new List<Developer>();
        private List<DeveloperRepository> developerRepository = new List<DeveloperRepository>();
        private List<File> files = new List<File>();
        private List<CommitFile> commitFiles = new List<CommitFile>();
        private List<Database.Models.Commit> commits = new List<Database.Models.Commit>();

        public class Data
        {
            public string commitSha { get; set; }
            public string commitAuthorName { get; set; }
            public string commitAuthorEmail { get; set; }
            public DateTimeOffset createdAt { get; set; }
            public string repositoryName { get; set; }
            public IEnumerable<string> filesNames { get; set; }
            public int additions { get; set; }
            public int deletions { get; set; }
        }

        public async Task Gathering(RepositoryAttribute [] attributes)
        {
            foreach(var attr in attributes)
            {
                var owner = attr.owner;
                var name = attr.name;
                var commits = await client.Repository.Commit.GetAll(owner, name);
                foreach(var commit in commits)
                {
                    var data = GetData(commit, owner, name);
                    


                }
            }
        }

        private Data GetData(GitHubCommit commit, string owner, string name)
        {
            var data = new Data
            {
                commitSha = commit.Commit.Sha,
                commitAuthorName = commit.Commit.Author.Name,
                commitAuthorEmail = commit.Commit.Author.Email,
                createdAt = commit.Commit.Author.Date,
                repositoryName = owner + "/" + name,
                filesNames = commit.Files.Select(x => x.Filename)
            };

            int additions = 0;
            int deletions = 0;

            var additionsArrayByCommit = commit.Files.Select(x => x.Additions);
            var deletionsArrayByCommit = commit.Files.Select(x => x.Deletions);
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
    }
}


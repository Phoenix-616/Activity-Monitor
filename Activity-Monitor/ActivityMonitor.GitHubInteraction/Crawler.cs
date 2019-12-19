﻿using Octokit;
using System.Threading.Tasks;
using ActivityMonitor.Database.Models.Git;
using System.Collections.Generic;
using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
using System.Linq;

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
        private List<File> files = new List<File>();
        private List<Database.Models.Commit> commits = new List<Database.Models.Commit>();
        private List<CommitFile> commitFiles = new List<CommitFile>();
        private List<DeveloperRepository> developerRepository = new List<DeveloperRepository>();

        public async Task Gathering(RepositoryAttribute [] attributes)
        {
            foreach(var attr in attributes)
            {
                var owner = attr.owner;
                var name = attr.name;
                var commits = await client.Repository.Commit.GetAll(owner, name);
                foreach(var commit in commits)
                {
                    var gitId = commit.Commit.Sha;
                    var authorName = commit.Commit.Author.Name;
                    var authorEmail = commit.Commit.Author.Email;
                    var createdAt = commit.Commit.Author.Date;
                    var repositoryName = owner + "/" + name;
                    var filesName = commit.Files.Select(x => x.Filename);
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
                }
            }
        }
    }
}


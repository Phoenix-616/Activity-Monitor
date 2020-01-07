using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityMonitor.GitHubInteraction
{
    public class Crawler
    {
        public readonly string NameOfApp = "Activity-Monitor";
        private readonly GitHubClient client;

        public class Data
        {
            public string CommitSha { get; set; }
            public string CommitAuthorName { get; set; }
            public string CommitAuthorLogin { get; set; }
            public string CommitAuthorEmail { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public string RepositoryName { get; set; }
            public IEnumerable<string> FilesNames { get; set; }
            public int Additions { get; set; }
            public int Deletions { get; set; }
        }
        private async Task<Data> GetData(GitHubCommit commit, string owner, string name)
        {
            var data = new Data
            {
                CommitSha = commit.Sha,
                CommitAuthorName = commit.Commit.Author.Name,
                CommitAuthorEmail = commit.Commit.Author.Email,
                CreatedAt = commit.Commit.Author.Date,
                RepositoryName = owner + "/" + name,
            };
            var author = commit.Author;
            var login = author == null ? null : author.Login;
            data.CommitAuthorLogin = login;

            var currentCommit = await client.Repository.Commit.Get(owner, name, data.CommitSha);
            var filesNames = currentCommit.Files.Select(x => x.Filename);
            data.FilesNames = filesNames;

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

            data.Additions = additions;
            data.Deletions = deletions;
            return data;
        }

        public async Task<List<Data>> DataGather(string owner, string name)
        {
            var list = new List<Data>();
            var commits = await client.Repository.Commit.GetAll(owner, name);
            foreach (var commit in commits)
            {
                var data = await GetData(commit, owner, name);
                list.Add(data);
            }
            return list;
        }

        public Crawler(string login, string password)
        {
            client = new GitHubClient(new ProductHeaderValue(NameOfApp));
            var basicAuth = new Credentials(login, password);
            client.Credentials = basicAuth;
        }
    }
}


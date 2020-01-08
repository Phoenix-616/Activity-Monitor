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
        private Dictionary<string, User> users = new Dictionary<string, User>();

        public class Data
        {
            public int DevID { get; set; }
            public int RepID { get; set; }
            public int CommitID { get; set; }
            public int FileID { get; set; }
            public string CommitSha { get; set; }
            public string CommitAuthorFirstName { get; set; }
            public string CommitAuthorLastName { get; set; }
            public string CommitAuthorLogin { get; set; }
            public string CommitAuthorEmail { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public string RepositoryName { get; set; }
            public IEnumerable<NameAndSha> NameAndSha { get; set; }
            public int Additions { get; set; }
            public int Deletions { get; set; }
        }
        public class NameAndSha
        {
            public string Name { get; set; }
            public int Id { get; set; }
        }
        private async Task<Data> GetData(GitHubCommit commit, string owner, string name)
        {
            var data = new Data
            {
                CommitID = GetIntIdByString(commit.Sha),

                CommitSha = commit.Sha,
                CommitAuthorEmail = commit.Commit.Author.Email,
                CreatedAt = commit.Commit.Author.Date,
                RepositoryName = owner + "/" + name,
            };
            await FillInfoByLogin(commit, data);
            
            var currentCommit = await client.Repository.Commit.Get(owner, name, data.CommitSha);
            var nameAndSha = currentCommit.Files.Select(x => new NameAndSha 
            {
                Name = x.Filename, 
                Id = GetIntIdByString(x.Sha)
            });
            data.NameAndSha = nameAndSha;

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

        private async Task FillInfoByLogin(GitHubCommit commit, Data data)
        {
            var author = commit.Author;
            if(author == null)
            {
                var fullName = commit.Commit.Author.Name.Split(" ");
                data.CommitAuthorFirstName = fullName[0];
                data.CommitAuthorLastName = fullName[1];
                data.CommitAuthorLogin = "";

                data.DevID = GetIntIdByString(fullName[0] + fullName[1]);
            }
            else
            {
                var login = author.Login;
                var user = await GetUser(login);
                var fullName = user.Name.Split(" ");
                data.CommitAuthorFirstName = fullName[0];
                data.CommitAuthorLastName = fullName[1];
                data.CommitAuthorLogin = login;

                data.DevID = author.Id;
            }
        }

        static public int GetIntIdByString(string id)
        {
            return (int)(id.GetHashCode());
        }

        private async Task<User> GetUser(string login)
        {
            User user;
            var result = users.TryGetValue(login, out user);
            if(!result)
            {
                user = await client.User.Get(login);
            }
            return user;
        }

        public async Task<List<Data>> DataGather(string owner, string name)
        {
            var list = new List<Data>();
            var commits = await client.Repository.Commit.GetAll(owner, name);
            var RepID = (int)(await client.Repository.Get(owner, name)).Id;
            foreach (var commit in commits)
            {
                var data = await GetData(commit, owner, name);
                data.RepID = RepID;
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

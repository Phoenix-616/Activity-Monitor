using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
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

        public class Entities
        {
            public List<Database.Models.Repository> repositories { get; set; }
            public List<Developer> developers { get; set; }
            public List<DeveloperRepository> developerRepositories{ get; set; }
            public List<FileInfo> files { get; set; }
            public List<CommitInfo> commits{ get; set; }
            public List<CommitFile> commitFiles{ get; set; }
        }

        public async Task<Entities> Get(string owner, string name)
        {
            var datas = await DataGather(owner, name);
            var entities = new Entities();

            entities.repositories = CreateRepos(datas);
            entities.developers = CreateDevs(datas);
            entities.developerRepositories = CreateDevReps(datas,
                                                           entities.repositories,
                                                           entities.developers);
            entities.files = CreateFiles(datas);
            entities.commits = CreateCommit(datas);
            entities.commitFiles = CreateCommitFiles(datas);

            return entities;
        }

        private List<CommitFile> CreateCommitFiles(List<Data> datas)
        {
            var list = new List<CommitFile>();
            foreach(var data in datas)
            {
                foreach(var file in data.NameAndSha)
                {
                    if(!list.Any(x => x.CommitId == data.CommitID && x.FileId == file.Id))
                    {
                        list.Add(new CommitFile
                        {
                            CommitId = data.CommitID,
                            FileId = data.FileID
                        });
                    }
                }
            }
            return list;
        }

        public class CommitInfo
        {
            public int Id { get; set; }
            public string GitId { get; set; }
            public string AuthorEmail { get; set; }
            public string RepositoryName { get; set; }
            public DateTimeOffset CreatedAt { get; set; }
            public int Created { get; set; }
            public int Deleted { get; set; }
        }

        private List<CommitInfo> CreateCommit(List<Data> datas)
        {
            var list = new List<CommitInfo>();
            foreach(var data in datas)
            {
                list.Add(new CommitInfo
                {
                    Id = data.CommitID,
                    GitId = data.CommitSha,
                    AuthorEmail = data.CommitAuthorEmail,
                    RepositoryName = data.RepositoryName,
                    CreatedAt = data.CreatedAt,
                    Created = data.Additions,
                    Deleted = data.Deletions
                }
                );
            }
            return list;
        }

        public class FileInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int RepId { get; set; }
            public string RepName { get; set; }
        }

        private List<FileInfo> CreateFiles(List<Data> datas)
        {
            var list = new List<FileInfo>();
            var delList = new List<string>();
            foreach(var data in datas)
            {
                foreach(var file in data.NameAndSha)
                {
                    var id = file.Id;
                    var name = file.Name;
                    var RepName = data.RepositoryName;
                    if(!list.Any(x => x.Id == id || (x.Name == name && x.RepName == RepName)))
                    {
                        if ("removed".Equals(file.Status))
                        {
                            delList.Add(name);
                        }
                        list.Add(new FileInfo { Id = id, Name = name, RepId = data.RepID, RepName = RepName });
                    }
                }
            }
            foreach(var delName in delList)
            {
                list.RemoveAll(x => x.Name == delName);
            }
            return list;
        }

        private List<DeveloperRepository> CreateDevReps(List<Data> datas,
                                                        List<Database.Models.Repository> repositories,
                                                        List<Developer> developers)
        {
            var list = new List<DeveloperRepository>();
            foreach(var data in datas)
            {
                if(!list.Any(x => x.RepositoryId == data.RepID && x.DeveloperId == data.DevID))
                {
                    list.Add(new DeveloperRepository { RepositoryId = data.RepID, DeveloperId = data.DevID,
                    Repository = repositories.Where(x => x.Name == data.RepositoryName).ToArray()[0],
                    Developer = developers.Where(x => x.Email == data.CommitAuthorEmail).ToArray()[0]
                    });
                }
            }
            return list;
        }

        private List<Developer> CreateDevs(List<Data> datas)
        {
            var devs = new List<Developer>();
            foreach (var data in datas)
            {
                if (!devs.Any(x => x.Email == data.CommitAuthorEmail))
                {
                    devs.Add(new Developer
                    { Id = data.DevID, FirstName = data.CommitAuthorFirstName,
                     LastName = data.CommitAuthorLastName, Login = data.CommitAuthorLogin,
                    Email = data.CommitAuthorEmail});
                }
            }
            return devs;
        }

        private List<Database.Models.Repository> CreateRepos(List<Data> datas)
        {
            var repos = new List<Database.Models.Repository>();
            foreach (var data in datas)
            {
                if (!repos.Any(x => x.Name == data.RepositoryName))
                {
                    repos.Add(new Database.Models.Repository
                    { Id = data.RepID, Name = data.RepositoryName });
                }
            }
            return repos;
        }

        public class NameAndSha
        {
            public string Status { get; set; }
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
            var nameAndSha = currentCommit.Files
            .Select(x => new NameAndSha 
            {
                Status = x.Status,
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
                var firstName = GetEmptyOrName(0, fullName);
                var lastName = GetEmptyOrName(1, fullName);
                data.CommitAuthorFirstName = firstName;
                data.CommitAuthorLastName = lastName;
                data.CommitAuthorLogin = "";

                data.DevID = GetIntIdByString(data.CommitAuthorEmail + firstName + lastName);
            }
            else
            {
                var login = author.Login;
                var user = await GetUser(login);
                var fullName = user.Name.Split(" ");
                var firstName = GetEmptyOrName(0, fullName);
                var lastName = GetEmptyOrName(1, fullName);
                data.CommitAuthorFirstName = firstName;
                data.CommitAuthorLastName = lastName;
                data.CommitAuthorLogin = login;

                //data.DevID = author.Id;
                data.DevID = GetIntIdByString(data.CommitAuthorEmail + firstName + lastName);
            }
        }

        private string GetEmptyOrName(int i, string[] vs)
        {
            try
            {
                return vs[i];
            }
            catch (IndexOutOfRangeException)
            {
                return "";
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

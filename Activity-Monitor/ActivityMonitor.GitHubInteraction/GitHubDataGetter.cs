using Octokit;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ActivityMonitor.GitHubInteraction
{

    class GitHubDataGetter
    {
        public readonly string NameOfApp = "Activity-Monitor";
        public readonly string GitAuthPath = "gitAuth.json";
        public readonly GitHubClient client;
        [DataContract]
        class AuthGitInfo
        {
            [DataMember]
            public string login { get; set; }
            [DataMember]
            public string password { get; set; }
        }
        public GitHubDataGetter()
        {
            AuthGitInfo info;
            using (Stream fs = GetStream())
            {
                var ser = new DataContractJsonSerializer(typeof(AuthGitInfo));
                fs.Position = 0;
                info = (AuthGitInfo)ser.ReadObject(fs);
            }
            client = new GitHubClient(new ProductHeaderValue("Activity-Monitor"));
            var basicAuth = new Credentials(info.login, info.password);
            client.Credentials = basicAuth;
        }

        private Stream GetStream()
        {
            return File.OpenRead("gitAuth.json");
        }

        

        private List<GitHubCommit> getCommitsOfContributor(
            IReadOnlyList<GitHubCommit> commits,
            string contributor)
        {
            var list = new List<GitHubCommit>();
            foreach(var commit in commits)
            {
                if(commit.Author != null && contributor.Equals(commit.Author.Login))
                {
                    list.Add(commit);
                }
            }
            return list;
        }

        public async Task<int> GetCodeSize(string ownerOfRepo, string nameOfRepo, string contributor)
        {
            int total = 0;
            var contibuters = await client.Repository.Statistics.GetContributors(ownerOfRepo, nameOfRepo);
            foreach (var cont in contibuters)
            {
                if (contributor.Equals(cont.Author.Login))
                {
                    foreach (var commitsByWeek in cont.Weeks)
                    {
                        total += commitsByWeek.Additions;
                        total -= commitsByWeek.Deletions;
                    }
                }
            }
            return total;
        }

        public async Task<IReadOnlyList<GitHubCommit>> GetAllCommits(string ownerOfRepo, string nameOfRepo)
        {
            var commits = await client.Repository.Commit.GetAll(ownerOfRepo, nameOfRepo);
            return commits;
        }

        public async Task<IReadOnlyList<Contributor>> GetContributers(string ownerOfRepo, string nameOfRepo)
        {
            var contibuters = await client.Repository.Statistics.GetContributors(ownerOfRepo, nameOfRepo);
            return contibuters;
        }

        public async Task<double> GetChurn(string ownerOfRepo, string nameOfRepo, string contributor)
        {
            int added = 0;
            int deleted = 0;
            var contibuters = await client.Repository.Statistics.GetContributors(ownerOfRepo, nameOfRepo);
            foreach (var cont in contibuters)
            {
                if (contributor.Equals(cont.Author.Login))
                {
                    foreach (var commitsByWeek in cont.Weeks)
                    {
                        added += commitsByWeek.Additions;
                        deleted += commitsByWeek.Deletions;
                    }
                }
            }
            return (deleted * 1.0) / added;
        }
    }
}

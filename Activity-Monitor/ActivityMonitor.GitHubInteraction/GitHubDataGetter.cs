using Octokit;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System;

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

        public async Task<int> GetCodeSize(string ownerOfRepo, string nameOfRepo, string contributer)
        {
            int total = 0;
            var contibuters = await client.Repository.Statistics.GetContributors(ownerOfRepo, nameOfRepo);
            foreach (var cont in contibuters)
            {
                if (contributer.Equals(cont.Author.Login))
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

        public async Task<double> GetChurn(string ownerOfRepo, string nameOfRepo, string contributer)
        {
            int added = 0;
            int deleted = 0;
            var contibuters = await client.Repository.Statistics.GetContributors(ownerOfRepo, nameOfRepo);
            foreach (var cont in contibuters)
            {
                if (contributer.Equals(cont.Author.Login))
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

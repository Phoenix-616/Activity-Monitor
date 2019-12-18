using Octokit;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ActivityMonitor.GitHubInteraction
{
    class GitHubDataGetter
    {
        public readonly string NameOfApp = "Activity-Monitor";
        public readonly string GitAuthPath = "gitAuth.json";
        private readonly GitHubClient client;
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

        public async Task<double> GetBusNumber (string ownerOfRepo, string nameOfRepo, string contributor)
        {
            var commits = await client.Repository.Commit.GetAll(ownerOfRepo, nameOfRepo);
            var quarterList = getQuarterList(commits);

            return 0.0;
        }

        private List<GitHubCommit> getQuarterList(IReadOnlyList<GitHubCommit> commits)
        {
            var list = new List<GitHubCommit>();
            foreach(var commit in commits )
            {
                var date = commit.Commit.Author.Date;
                var nowDate = DateTimeOffset.Now;
                var quarter = new TimeSpan(90, 0, 0, 0, 0);
                if (nowDate - date <= quarter)
                {
                    list.Add(commit);
                }
            }
            return list;
        }

        /// <summary>
        /// return age of the oldest file
        /// </summary>
        public async Task<TimeSpan> GetAgeOfProject (
            string ownerOfRepo, string nameOfRepo)
        {
            var earlystDates = DateTimeOffset.MaxValue;
            var files = await client.Git.Tree.GetRecursive(ownerOfRepo, nameOfRepo, "master");
            var filePaths = files.Tree.Select(x => x.Path);
            foreach (var path in filePaths)
            {
                var request = new CommitRequest { Path = path };
                var commitsForFile = await client.Repository.Commit.GetAll(ownerOfRepo, nameOfRepo, request);
                var latestDate = new DateTimeOffset();
                foreach(var commit in commitsForFile)
                {
                    var commitDate = commit.Commit.Author.Date;
                    if (DateTimeOffset.Compare(latestDate, commitDate) < 0)
                    {
                        latestDate = commitDate;
                    }
                }
                if(DateTimeOffset.Compare(latestDate, earlystDates) < 0 )
                {
                    earlystDates = latestDate;
                }
            }
            return DateTime.Now - earlystDates.DateTime;
        }

        public async Task<int> GetActiveDays(string ownerOfRepo, string nameOfRepo, string contributor)
        {
            var commits = await client.Repository.Commit.GetAll(ownerOfRepo, nameOfRepo);
            var commitsOfContributor = GetCommitsOfContributor(commits, contributor);

            int counter = 0;

            var previouslyDate = new DateTimeOffset();
            var previouslyYear = previouslyDate.Year;
            var previouslyDayOfYear = previouslyDate.DayOfYear;
             
            foreach (var commit in commitsOfContributor)
            {
                var commitDate = commit.Commit.Author.Date;
                var commitYear = commitDate.Year;
                var commitDayOfYear = commitDate.DayOfYear;

                if (previouslyDayOfYear != commitDayOfYear || previouslyYear != commitYear)
                {
                    counter++;
                    previouslyYear = commitDate.Year;
                    previouslyDayOfYear = commitDate.DayOfYear;
                }
            }
            return counter;
        }

        private List<GitHubCommit> GetCommitsOfContributor(
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

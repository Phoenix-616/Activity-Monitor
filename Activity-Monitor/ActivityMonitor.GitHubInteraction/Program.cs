using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ActivityMonitor.GitHubInteraction
{
    class Program
    {
        [DataContract]
        class AuthGitInfo
        {
            [DataMember]
            public string login { get; set; }
            [DataMember]
            public string password { get; set; }
        }

        public static async Task Main()
        {
            var getter = new GitHubDataGetter();
            var u = await getter.client.User.Get("ogresed");
            Console.Write(u.PublicRepos);
            /*var getter = GitHubDataGetter("123", "12312");
            var client = new GitHubClient(new ProductHeaderValue("Activity-Monitor"));
            client = await GitHubAuthorization.GetGitHubClient("ogresed", "4pieylhw");
            var user = await client.User.Get("ogresed");

            var repos = await client.Repository.GetAllForUser("ogresed");
            var w = repos.GetEnumerator();
            var rep = w.Current;
            Console.WriteLine(rep.Name);

            //var client = new GitHubClient(new ProductHeaderValue("joe307bad-commits"));

            var repository = await client.Repository.Commit.GetAll("joe307bad", "portfolio_wp");

            var commitsFiltered = repository.Select(async (_) =>
            {
                return await client.Repository.Commit.Get("joe307bad", "portfolio_wp", _.Sha);
            }).ToList();

            var commits = await Task.WhenAll(commitsFiltered);

            Console.WriteLine("\n\n" +
                "name - {0} \n" +
                "has public repositories - {1} \n" +
                "go check out their profile at {2}\n" +
                "private repositories - {3}\n" +
                "{4}\n" +
                "{5}"
                ,
                user.Name,
                user.PublicRepos,
                user.Url,
                user.OwnedPrivateRepos,
                user.Followers,
                user.UpdatedAt);*/
        }
    }
}

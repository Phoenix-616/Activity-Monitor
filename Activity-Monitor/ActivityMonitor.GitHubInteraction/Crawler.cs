using Octokit;
using System.Threading.Tasks;


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

        public async Task Gathering(Repository [] repositories)
        {
            foreach (var repo in repositories)
            {
                
            }
        }
    }
}

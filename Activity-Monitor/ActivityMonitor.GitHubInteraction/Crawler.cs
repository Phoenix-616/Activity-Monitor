using Octokit;
namespace ActivityMonitor.GitHubInteraction
{
    class Crawler
    {
        public readonly string NameOfApp = "Activity-Monitor";
        private readonly GitHubClient client;       

        public Crawler(string login, string password)
        {
            client = new GitHubClient(new ProductHeaderValue(NameOfApp));
            var basicAuth = new Credentials(login, password);
            client.Credentials = basicAuth;
        }
    }
}


using Octokit;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;

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
    }
}

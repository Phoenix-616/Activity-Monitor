using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace ActivityMonitor.GitHubInteraction
{
    class Program
    {
        public readonly string GitAuthPath = "gitAuth.json";             
        public static void InitJson()
        {
            var am = new RepositoryAttribute
            {
                owner = "Phoenix-616",
                name = "Activity-Monitor"
            };
            var cg = new RepositoryAttribute
            {
                owner = "ogresed",
                name = "CardGame"
            };
            RepositoryAttribute[] repositorys = new RepositoryAttribute[2];
            repositorys[0] = am;
            repositorys[1] = cg;
            var a = new AuthGitInfo
            {
                login = "ogresed",
                password = "enterPasswod",
                repositories = repositorys
            };

            var stream1 = File.OpenWrite("gitAuth.json");
            var ser = new DataContractJsonSerializer(typeof(AuthGitInfo));
            ser.WriteObject(stream1, a);
        }
        public static async Task Main()
        {
            AuthGitInfo info;
            using (Stream fs = GetStream())
            {
                var ser = new DataContractJsonSerializer(typeof(AuthGitInfo));
                fs.Position = 0;
                info = (AuthGitInfo)ser.ReadObject(fs);
            }
            var crawler = new Crawler(info.login, info.password);
            var models = await crawler.Gathering(info.repositories);
        }

        private static Stream GetStream()
        {
            return File.OpenRead("gitAuth.json");
        }
    }
}

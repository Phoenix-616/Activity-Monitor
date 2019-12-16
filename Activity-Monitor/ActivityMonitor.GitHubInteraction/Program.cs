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

            Console.Write(await getter.GetChurn("ogresed", "CardGame", "ogresed"));
        }
    }
}

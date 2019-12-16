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
            
            Console.Write(await getter.GetChurn("Phoenix-616", "Activity-Monitor", "momikenSneg"));
            Console.WriteLine();
            Console.Write(await getter.GetCodeSize("Phoenix-616", "Activity-Monitor", "Phoenix-616"));
            Console.WriteLine();

            var contr = await getter.GetContributers("Phoenix-616", "Activity-Monitor");
            foreach(var c in contr)
            {
                Console.WriteLine(c.Author.Login);

            }



            var punch = await getter.client.Repository.Statistics.GetPunchCard("ogresed", "CardGame");
            var part = await getter.client.Repository.Statistics.GetParticipation("ogresed", "CardGame");
            
            var commAct = await getter.client.Repository.Statistics.GetCommitActivity("ogresed", "CardGame");
        }
    }
}

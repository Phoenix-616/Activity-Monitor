using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ActivityMonitor.GitHubInteraction
{
    class Program
    {
        public static async Task Main()
        {
            var getter = new GitHubDataGetter();
            
            //Console.WriteLine(await getter.GetAll("Phoenix-616", "Activity-Monitor", "ogresed"));
            Console.WriteLine();
        }
    }
}

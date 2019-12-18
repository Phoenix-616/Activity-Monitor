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
            
            Console.WriteLine(await getter.GetActiveDays("Phoenix-616", "Activity-Monitor", "Phoenix-616"));
            Console.WriteLine();
        }
    }
}

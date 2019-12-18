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

            var d = await getter.GetAgeOfProject("Phoenix-616", "Activity-Monitor");
            Console.WriteLine($"{d}");
            Console.WriteLine();
        }
    }
}

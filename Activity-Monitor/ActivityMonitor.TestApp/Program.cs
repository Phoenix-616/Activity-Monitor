using System;
using System.Threading.Tasks;

namespace ActivityMonitor.TestApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new PMT.PMT("e.snegireva", "novgorodskii98", new Uri("https://ai.nsu.ru"));
            var proj = await client.GetProjects();

            Console.WriteLine("Hello World!");
        }
    }
}

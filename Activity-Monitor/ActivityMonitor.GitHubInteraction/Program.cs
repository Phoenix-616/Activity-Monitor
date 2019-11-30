using System;
using Octokit;

namespace ActivityMonitor.GitHubInteraction
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientId = "6efbf777d1c4c3f1862e";
            var clientSecret = "a460bed91ffc7e1b3b9ace4d39ddfc9aaef70654";
            var client = new GitHubClient(new ProductHeaderValue("Activity Monitor"));
            Console.ReadKey();
        }
    }
}

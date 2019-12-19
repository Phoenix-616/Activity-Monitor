using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
using System.Collections.Generic;

namespace ActivityMonitor.GitHubInteraction
{
    public class Models
    {
        private List<Repository> repositories;
        private List<Developer> developers;
        private List<DeveloperRepository> developerRepository;
        private List<File> files;
        private List<CommitFile> commitFiles;
        private List<Database.Models.Commit> commits;

        public Models()
        {
            repositories = new List<Repository>();
            developers = new List<Developer>();
            developerRepository = new List<DeveloperRepository>();
            files = new List<File>();
            commitFiles = new List<CommitFile>();
            commits = new List<Database.Models.Commit>();
        }
    }
}

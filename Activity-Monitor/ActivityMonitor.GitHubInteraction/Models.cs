using ActivityMonitor.Database.Models;
using ActivityMonitor.Database.Models.Git.ActivityMonitor.Database.Models;
using System.Collections.Generic;

namespace ActivityMonitor.GitHubInteraction
{
    public class Models
    {
        public List<Repository> repositories;
        public List<Developer> developers;
        public List<DeveloperRepository> developerRepository;
        public List<File> files;
        public List<CommitFile> commitFiles;
        public List<Database.Models.Commit> commits;

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

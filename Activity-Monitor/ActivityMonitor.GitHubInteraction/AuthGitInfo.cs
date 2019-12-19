using System.Runtime.Serialization;

namespace ActivityMonitor.GitHubInteraction
{
    [DataContract]
    class AuthGitInfo
    {
        [DataMember]
        public string login { get; set; }
        [DataMember]
        public string password { get; set; }
        [DataMember]
        public RepositoryAttribute[] repositories { get; set; }
    }
}

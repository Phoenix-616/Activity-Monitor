using System.Runtime.Serialization;

namespace ActivityMonitor.GitHubInteraction
{
    [DataContract]
    public class RepositoryAttribute
    {
        [DataMember]
        public string owner { get; set; }
        [DataMember]
        public string name { get; set; }
    }
}

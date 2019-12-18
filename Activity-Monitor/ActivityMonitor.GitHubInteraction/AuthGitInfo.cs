using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

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
        public Repository[] repositories { get; set; }
    }
}

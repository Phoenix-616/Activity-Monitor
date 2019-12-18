using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ActivityMonitor.GitHubInteraction
{
    [DataContract]
    public class Repository
    {
        [DataMember]
        public string owner { get; set; }
        [DataMember]
        public string name { get; set; }
    }
}

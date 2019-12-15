using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;

namespace ActivityMonitor.PMT
{
    class PMT : APMT
    {
        public PMT(string login, string password, Uri pmtUri) : base(login, password, pmtUri)
        {
        }

        public override async Task<Project[]> GetProjects()
        {
            var serializer = new DataContractJsonSerializer(typeof(Projects));
            var streamTask = base._client.GetStreamAsync("/projects.json");
            var projects = serializer.ReadObject(await streamTask) as Projects;

            return projects.projects;
        }

        public override async Task<Membership[]> GetProjectUsers(int projectId)
        {
            string url = $"/projects/{projectId}/memberships.json";
            var serializer = new DataContractJsonSerializer(typeof(Memberships));
            var streamTask = base._client.GetStreamAsync(url);
            var memberships = serializer.ReadObject(await streamTask) as Memberships;

            return memberships.memberships;
        }

        public override async Task<Issue[]> GetTaskList(int projectId)
        {
            string url = $"/issues.json?project_id={projectId}";
            var serializer = new DataContractJsonSerializer(typeof(Issues));
            var streamTask = base._client.GetStreamAsync(url);
            var issues = serializer.ReadObject(await streamTask) as Issues;

            return issues.issues;
        }

        public override async Task<IssueHistory[]> GetTaskHistory(int issueId)
        {
            string url = $"/issues/{issueId}.json?include=journals";
            var serializer = new DataContractJsonSerializer(typeof(ForProjectHistory));
            var streamTask = base._client.GetStreamAsync(url);
            var issueHistory = serializer.ReadObject(await streamTask) as ForProjectHistory;

            return issueHistory.issue.journals;
        }
    }


    class Projects
    {
        public Project[] projects { get; set; }
    }

    class ForProjectHistory
    {
        public Issue issue { get; set; }
    }

    class Memberships
    {
        public Membership[] memberships { get; set; }
    }

    class Issues
    {
        public Issue[] issues { get; set; }
    }

    public class Project
    {
        public int id { get; set; }
        public string name { get; set; }
        public string identifier { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public CustomFields[] custom_fields { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
    }

    public class CustomFields
    {
        public int id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Membership
    {
        public int id { get; set; }
        public IdName user { get; set; }
        public IdName[] roles { get; set; }
    }

    public class Issue
    {
        public int id { get; set; }
        public IdName tracker { get; set; }
        public IdName status { get; set; }
        public IdName priority { get; set; }
        public IdName author { get; set; }
        public IdName assigned_to { get; set; }
        public string subject { get; set; }
        public DateTime start_date { get; set; }
        public DateTime due_date { get; set; }
        public int done_ratio { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public IssueHistory[] journals { get; set; }
    }

    public class IssueHistory
    {
        public int id;
        public IdName user;
        public string notes;
        public DateTime created_on;
    }

    public class Details
    {
        public string property { get; set; }
        public string name { get; set; }
        public string old_value { get; set; }
        public string new_value { get; set; }
    }

    public class IdName
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}

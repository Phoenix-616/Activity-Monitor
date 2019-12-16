using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ActivityMonitor.PMT
{
    /// <summary>
    /// Common abstract class for project managment tools
    /// </summary>
    public abstract class APMT
    {
        protected HttpClient _client;
        public APMT(string login, string password, Uri pmtUri)
        {
            _client = new HttpClient
            {
                BaseAddress = pmtUri
            };
            var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public abstract Task<Project[]> GetProjects();
        public abstract Task<Membership[]> GetProjectUsers(int id);
        public abstract Task<Issue[]> GetTaskList(int id);
        public abstract Task<IssueHistory[]> GetTaskHistory(int id);
    }
}



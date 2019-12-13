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

        public abstract Task<string[]> GetProjects();
        public abstract Task<string[]> GetProjectUsers(string project);
        public abstract Task<string[]> GetTaskList(string project);
        public abstract Task<string[]> GetTaskHistory(string task);
    }

    internal class A : APMT
    {
        public A(string login, string password, Uri pmtUri) : base(login, password, pmtUri)
        {
        }

        public override Task<string[]> GetProjects()
        {
            throw new NotImplementedException();
        }

        public override async Task<string[]> GetProjectUsers(string project)
        {
            throw new NotImplementedException();
        }
        public override Task<string[]> GetTaskList(string project)
        {
            throw new NotImplementedException();
        }
        public override Task<string[]> GetTaskHistory(string task)
        {
            throw new NotImplementedException();
        }

    }
}



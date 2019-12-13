using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ActivityMonitor.PMT
{
    class PMT : APMT
    {
        public PMT(string login, string password, Uri pmtUri) : base(login, password, pmtUri)
        {
        }

        public override Task<string[]> GetProjects()
        {
            throw new NotImplementedException();
        }

        public override Task<string[]> GetProjectUsers(string project)
        {
            throw new NotImplementedException();
        }

        public override Task<string[]> GetTaskHistory(string task)
        {
            throw new NotImplementedException();
        }

        public override Task<string[]> GetTaskList(string project)
        {
            throw new NotImplementedException();
        }
    }
}

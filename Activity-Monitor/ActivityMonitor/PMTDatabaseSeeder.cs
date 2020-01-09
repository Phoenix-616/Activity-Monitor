using System;
using System.Collections.Generic;
using System.Text;
using ActivityMonitor.Database.Models;
using ActivityMonitor.Database;
using ActivityMonitor.PMT;
using Project = ActivityMonitor.Database.Models.Project;
using System.Threading.Tasks;
using Membership = ActivityMonitor.Database.Models.Membership;
using Issue = ActivityMonitor.Database.Models.Issue;
using Task = ActivityMonitor.PMT.Task;
using System.Linq;

namespace ActivityMonitor
{
    class PMTDatabaseSeeder
    {
        private ActivityContext context;
        private List<string> prj;
        private APMT pmt;
        Random random = new Random();
        public PMTDatabaseSeeder(ActivityContext c, List<string> projects, string name, string password, string url)
        {
            context = c;
            prj = projects;
            pmt = new PMT.PMT(name, password, new Uri(url));
        }

        public async System.Threading.Tasks.Task Seed()
        {
            var pr = await FillProjects();
            foreach (Project proj in pr){
                await FillMembership(proj.Id);
                var iss = await FillIssues(proj.Id);

                foreach (Issue issue in iss)
                {
                    await FillIssueHistory(issue);
                }

            }
            await context.SaveChangesAsync();
        }

        private async Task<List<Project>> FillProjects()
        {
            List<Project> save = new List<Project>();
            var projects = await pmt.GetProjects();
            for (int i = 0; i < projects.Length; i++)
            {
                if (!prj.Contains(projects[i].name))
                    continue;
                Project one = new Project
                {
                    Id = projects[i].id,
                    Name = projects[i].name,
                    Description = projects[i].description,
                    Status = projects[i].status,
                    Created_on = projects[i].created_on,
                    Updated_on = projects[i].updated_on
                };
                if (!context.Projects.Any(x => x.Id == one.Id))
                {
                    context.Projects.Add(one);
                }
                save.Add(one);
            }
            return save;
        }

        private async System.Threading.Tasks.Task FillMembership(int projId)
        {
            var memberships = await pmt.GetProjectUsers(projId);

            for (int i = 0; i < memberships.Length; i++)
            {
                Membership one = new Membership
                {
                    Id = memberships[i].user.id,
                    Name = memberships[i].user.name
                };

                ProjectMembership pm = new ProjectMembership
                {
                    ProjectId = projId,
                    MembershipId = memberships[i].user.id
                };
                /*
                var f = await context.Memberships.FindAsync(one);
                 
                if (f == null)
                */
                if (!context.Memberships.Any(x => x.Id == one.Id))
                {
                    context.Memberships.Add(one);
                }
                if (!context.ProjectMemberships.Any(x => x.MembershipId == pm.MembershipId &&
                x.ProjectId == pm.ProjectId))
                {
                    context.ProjectMemberships.Add(pm);
                }
            }
        }

        private async Task<List<Issue>> FillIssues(int projId)
        {
            int offset = 0;
            List<Issue> save = new List<Issue> ();

            Tasks issues_list;
            Task[] issues;

            do
            {
                issues_list = await pmt.GetTaskList(projId, offset);
                issues = issues_list.issues;


                for (int i = 0; i < issues.Length; i++)
                {
                    if (issues[i].tracker.name != "Task")
                        continue;

                    Issue one = new Issue
                    {
                        Id = issues[i].id,
                        TrackerName = issues[i].tracker.name,
                        AuthorId = issues[i].author.id,
                        StartDate = issues[i].start_date,
                        DueDate = issues[i].due_date,
                        ProjectId = projId,
                        MembershipId = issues[i].assigned_to.id
                    };
                    if (!context.Issues.Any(x => x.Id == one.Id))
                    {
                        context.Issues.Add(one);
                    }
                    save.Add(one);
                }


                //issues_list = await pmt.GetTaskList(projId, offset);
                //issues = issues_list.issues;
                offset = issues_list.offset + issues_list.limit;
            } while (offset < issues_list.total_count);

            return save;
        }

        //TODO: добавлять в историю первое изменение на асайн то

        private async System.Threading.Tasks.Task FillIssueHistory(Issue issue)
        {
            var history = await pmt.GetTaskHistory(issue.Id);

            Journal one;

            for (int i = 0; i < history.Length; i++)
            {
                if (history[i].details.Length > 0)
                {
                    one = new Journal
                    {
                        Id = history[i].id,
                        AuthorId = history[i].user.id,
                        Notes = history[i].notes,
                        CreatedOn = history[i].created_on,
                        NameChange = history[i].details[0].name,
                        OldValue = history[i].details[0].old_value,
                        NewValue = history[i].details[0].new_value,
                        IssueId = issue.Id
                    };
                    if (!context.Journals.Any(x => x.Id == one.Id))
                    {
                        context.Journals.Add(one);
                    }
                }
            }
            
        }

        private string GetStatus(int id)
        {
            switch (id)
            {
                case 1:
                    return "New";
                case 2:
                    return "In Progress";
                case 3:
                    return "Resolved";
                case 4:
                    return "Feedback";
                case 5:
                    return "Closed";
                case 6:
                    return "Rejected";
                case 7:
                    return "Need more info";
                case 8:
                    return "Analysis";
                case 9:
                    return "Analysis completed";
                case 10:
                    return "Implementation";
                case 11:
                    return "Implemented";
                case 12:
                    return "Postponed";
                case 13:
                    return "Triggered";
                case 14:
                    return "Mitigated";
                case 15:
                    return "Canceled";
            }
            return "non";
        }
    }
}

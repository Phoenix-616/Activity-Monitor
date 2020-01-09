using Newtonsoft.Json;
using System;
using System.IO;
using ActivityMonitor.Database;
using System.Threading.Tasks;



//АКТУАЛЬНЫЙ
namespace ActivityMonitor
{
    class Program
    {
        static async Task Main(string[] args)
        {            
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            using (StreamReader r = new StreamReader(args[0]))
            {
                string json = r.ReadToEnd();
                var config = JsonConvert.DeserializeObject<Configuration>(json);

                ActivityContext context = new ActivityContext();
                /*PMTDatabaseSeeder seeder = new PMTDatabaseSeeder(
                    context,
                    config.PMTProjects,
                    config.PMTLogin,
                    config.PMTPassword,
                    config.PMTUri
                    );

                await seeder.Seed();*/

                var gitSeeder = new GitDatabaseSeeder(context,
                                                      config.GitLogin,
                                                      config.GitPassword,
                                                      config.GitRepositories);
                await gitSeeder.Seed();
            }
        }
    }
}

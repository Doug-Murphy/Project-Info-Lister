using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectReferencesBuilder.Helpers;
using ProjectReferencesBuilder.Helpers.Interface.WarningHelpers;
using ProjectReferencesBuilder.Helpers.WarningHelpers;
using ProjectReferencesBuilder.Services;
using ProjectReferencesBuilder.Services.Interface;
using System;

namespace ProjectReferencesBuilder
{
    public class Program
    {
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IEndOfLifeWarningHelper, EndOfLifeWarningHelper>();
                    services.AddSingleton<IProjectInfoService, ProjectInfoService>();
                    services.AddSingleton<IProjectStyleWarningHelper, ProjectStyleWarningHelper>();
                    services.AddSingleton<IRunnerService, RunnerService>();
                    services.AddSingleton<IWarningsService, WarningsService>();
                });
        }
 
        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var runnerService = host.Services.GetRequiredService<IRunnerService>();

            string solutionFilePath = null;
            if (args.Length == 1)
            {
                solutionFilePath = args[0];
            }

            while (FileHelper.GetFileExtension(solutionFilePath) != ".sln")
            {
                Console.Write("Enter the full path to the solution (.sln) file: ");
                solutionFilePath = Console.ReadLine();
            }

            solutionFilePath = solutionFilePath.Trim('"');

            var results = runnerService.GetProjectDetails(solutionFilePath);

            runnerService.WriteResultsToConsole(results);
        }
    }
}

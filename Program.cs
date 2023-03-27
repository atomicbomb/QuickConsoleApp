using QuickConsoleApp;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static async Task Main(string[] args)
    {
        #region Configure appSettings.json
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();
        #endregion

        var workingDirectory = config["WorkingDirectory"];
        var personalProjectsDirectory = config["PersonalProjectsDirectory"];

        var projectName = Helper.GetProjectName();
        var isPersonalProject = Helper.IsPersonalProject();

        if (isPersonalProject) {
            workingDirectory += personalProjectsDirectory;
        }

        await Helper.GenerateConsoleProject(workingDirectory, projectName);

        await Helper.OpenConsoleProject(workingDirectory, projectName);

        Console.WriteLine("Exiting...");
        Thread.Sleep(1500);
        Environment.Exit(0);
    }
}
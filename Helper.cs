using CliWrap;
using CliWrap.Buffered;

namespace QuickConsoleApp;

internal static class Helper
{
    internal static string GetProjectName()
    {
        Console.Clear();
        Console.Write("Please enter the project name: ");
        var projectName = Console.ReadLine();

        if (string.IsNullOrEmpty(projectName))
        {
            Console.WriteLine("Exiting the Application....");
            Thread.Sleep(1500);
            Environment.Exit(0);
        }

        return projectName;
    }

    internal static bool IsPersonalProject()
    {
        Console.Clear();
        Console.Write("Is this a personal project? (y/n): ");
        var personalProject = Console.ReadKey();

        if (personalProject.KeyChar == 'y')
        {
            return true;
        }

        return false;
    }

    internal static async Task GenerateConsoleProject(string workingDirectory, string projectName)
    {
        Console.Clear();
        Console.WriteLine("Generating console application project, please wait...");

        var results = await Cli.Wrap("dotnet")
            .WithArguments(new[] { "new", "console", "-o", $"{projectName}" })
            .WithWorkingDirectory(workingDirectory)
            .ExecuteBufferedAsync();

            if (!string.IsNullOrEmpty(results.StandardError))
            {
                Console.WriteLine($"Error creating console application project: {results.StandardError}");
                Console.WriteLine("Exiting the Application....");
                Thread.Sleep(1500);
                Environment.Exit(0);
            }
            else
            {
                var createdIn = (results.ExitTime - results.StartTime).TotalSeconds;
                Console.WriteLine($"Project '{projectName}' successfully created in {createdIn} seconds");
            }

        Console.WriteLine("");
        Console.WriteLine("Restoring project dependencies, please wait...");

        results = await Cli.Wrap(@"dotnet")
            .WithArguments(new[] { "restore" })
            .WithWorkingDirectory(@$"{workingDirectory}\{projectName}")
            .ExecuteBufferedAsync();

        Console.Clear();

        if (!string.IsNullOrEmpty(results.StandardError))
        {
            Console.WriteLine($"Error resolving project dependencies: {results.StandardError}");
            Console.WriteLine("Exiting the Application....");
            Thread.Sleep(1500);
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine($"Project dependencies successfully restored. Opening VSCode...");
        }
    }

    internal static async Task OpenConsoleProject(string workingDirectory, string projectName)
    {
        var results = await Cli.Wrap(@"code")
            .WithArguments(new[] { "." })
            .WithWorkingDirectory(@$"{workingDirectory}\{projectName}")
            .ExecuteBufferedAsync();

        Console.Clear();

        if (!string.IsNullOrEmpty(results.StandardError)) {
            Console.WriteLine($"Error opening VSCode: {results.StandardError}");
        } else {
            Console.WriteLine("Console Application successfully created.");
        }
    }
}
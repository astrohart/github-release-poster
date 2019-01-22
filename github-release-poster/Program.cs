using System;
using System.Diagnostics;
using System.Reflection;
using github_release_poster.Properties;

namespace github_release_poster
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            CommandLineInfo.Instance.ParseCommandLine(args);

            if (!ProcessCommandLine())
            {
                PrintUsageMessage();
                Environment.Exit(Resources.FAILED_TO_PROCESS_COMMAND_LINE);
            }

            Environment.Exit(Resources.EXIT_SUCCESS);
        }

        public static void PrintUsageMessage()
        {
            PrintVersionNumber();
            Console.WriteLine();
            Console.WriteLine(Resources.Usage);
            Console.WriteLine();
            Console.WriteLine(Resources.AllSwitchesCaseSensitive);
            Console.WriteLine();
            Console.WriteLine(Resources.BODY_SWITCH_USAGE);
            Console.WriteLine(Resources.NAME_SWITCH_USAGE);
            Console.WriteLine(Resources.RELEASE_ASSET_DIR_SWITCH_USAGE);
            Console.WriteLine(Resources.TAG_NAME_SWITCH_USAGE);
            Console.WriteLine(Resources.TARGET_BRANCH_SWITCH_USAGE);
            Console.WriteLine(Resources.DefaultTargetBranchIsMaster);
            Console.WriteLine(Resources.USER_ACCESS_TOKEN_SWITCH_USAGE);
            Console.WriteLine(Resources.IS_DRAFT_SWITCH_USAGE);
            Console.WriteLine(Resources.IS_PRE_RELEASE_SWITCH_USAGE);
            Console.WriteLine(Resources.REPO_NAME_SWITCH_USAGE);
            Console.WriteLine(Resources.REPO_OWNER_SWITCH_USAGE);
            Console.WriteLine(Resources.VERSION_SWITCH_USAGE);
            Console.WriteLine(Resources.NO_ZIP_SWITCH_USAGE);
            Console.WriteLine();
            Console.WriteLine(Resources.SwitchRequired, Resources.NAME_SWITCH);
            Console.WriteLine(Resources.SwitchRequired, Resources.RELEASE_ASSET_DIR_SWITCH);
            Console.WriteLine(Resources.SwitchRequired, Resources.TAG_NAME_SWITCH);
            Console.WriteLine(Resources.SwitchRequired, Resources.TARGET_BRANCH_SWITCH);
            Console.WriteLine(Resources.SwitchRequired, Resources.USER_ACCESS_TOKEN_SWITCH);
            Console.WriteLine(Resources.SwitchRequired, Resources.REPO_NAME_SWITCH);
            Console.WriteLine(Resources.SwitchRequired, Resources.REPO_OWNER_SWITCH);
        }

        public static void PrintVersionNumber()
        {
            var execAssembly = Assembly.GetCallingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(execAssembly.Location);

            var name = execAssembly.GetName();

            Console.WriteLine(Resources.VersionString,
                name.Name.Replace(".exe", String.Empty),
                name.Version);
            Console.WriteLine(fileVersionInfo.LegalCopyright);
        }

        private static bool ProcessCommandLine()
        {
            if (CommandLineInfo.Instance.ShouldDisplayVersion)
            {
                // By now, we've already displayed the version number of the program
                // to the user as called for by the --version switch, so exit the program
                // here with a 'success' error code.
                Environment.Exit(Resources.EXIT_SUCCESS);
            }

            var newRelease = NewReleaseFactory.CreateNewRelease(
                CommandLineInfo.Instance.Body,
                CommandLineInfo.Instance.IsDraft,
                CommandLineInfo.Instance.Name,
                CommandLineInfo.Instance.IsPreRelease,
                CommandLineInfo.Instance.TagName,
                CommandLineInfo.Instance.TargetBranch
            );

            if (!GitHubReleaseValidator.IsReleaseValid(newRelease))
            {
                Console.WriteLine(Resources.FailedValidateReleaseMetadata);
                return false; // Failed to validate release information
            }

            GitHubReleasePoster.PostNewRelease(
                CommandLineInfo.Instance.RepoName,
                CommandLineInfo.Instance.RepoOwner,
                CommandLineInfo.Instance.UserAccessToken,
                CommandLineInfo.Instance.ReleaseAssetDir,
                newRelease
            );

            return true;
        }
    }
}
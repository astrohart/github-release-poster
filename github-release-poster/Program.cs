using github_release_poster.Properties;
using System;
using System.Diagnostics;
using System.Reflection;

namespace github_release_poster
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SafeClearConsole();

            // Examine the command line for valid inputs, and fill the properties of the
            // CommandLineInfo object accordingly.
            if (!CommandLineInfo.Instance.ParseCommandLine(args))
            {
                Environment.Exit(Resources.FAILED_TO_PARSE_COMMAND_LINE);
            }

            // Take action based on what the user passed on the command line.
            if (!ProcessCommandLine())
            {
                PrintUsageMessage();
                Environment.Exit(Resources.FAILED_TO_PROCESS_COMMAND_LINE);
            }

            // If we are here, the program ran successfully.  Exit with a success error code.
            Environment.Exit(Resources.EXIT_SUCCESS);
        }

        public static void PrintUsageMessage()
        {
            // clear the console -- IF we are not running unit tests!
            SafeClearConsole();

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
                name.Name.Replace(".exe", string.Empty),
                name.Version);
            Console.WriteLine(fileVersionInfo.LegalCopyright);
        }

        /// <summary>
        /// Clears all output from the user's console.
        /// </summary>
        public static void SafeClearConsole()
        {
            try
            {
                Console.Clear(); // can throw exceptions during unit tests -- but we don't care
            }
            catch
            {
                //Ignored.
            }
        }

        /// <summary>
        /// Called to examine the properties of the <see cref="T:github_release_poster.CommandLineInfo"/> object
        /// and then take appropriate action.
        /// </summary>
        /// <returns>True if the command line was processed successfully; false otherwise.</returns>
        private static bool ProcessCommandLine()
        {
            if (CommandLineInfo.Instance.ShouldDisplayVersion)
            {
                // By now, we've already displayed the version number of the program
                // to the user as called for by the --version switch.  So, we are done.
                return true;
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
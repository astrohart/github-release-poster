using github_release_poster.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace github_release_poster
{
    /// <summary>
    /// Encapsulates the command-line parameters of this app.
    /// </summary>
    public class CommandLineInfo
    {
        /// <summary>
        /// Holds an reference to the one and only instance of <see cref="T:github_release_poster.CommandLineInfo"/>.
        /// </summary>
        private static CommandLineInfo _theCommandLineInfo;

        /// <summary>
        /// Constructs a new instance of <see cref="T:github_release_poster.CommandLineInfo"/> and returns a reference to it.
        /// </summary>
        /// <remarks>
        /// This constructor is marked protected, since this class is a singleton.
        /// </remarks>
        protected CommandLineInfo()
        {
            Clear();
        }

        /// <summary>
        /// Gets a reference to the one and only instance of <see cref="T:github_release_poster.CommandLineInfo"/>.
        /// </summary>
        public static CommandLineInfo Instance => _theCommandLineInfo ?? (_theCommandLineInfo = new CommandLineInfo());

        /// <summary>
        /// Gets or sets text for the body of the release.  May be blank.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this release is a draft.
        /// </summary>
        public bool IsDraft { get; set; }

        /// <summary>
        /// Gets or sets a flag that indicates whether the release is pre-release.
        /// </summary>
        public bool IsPreRelease { get; set; }

        /// <summary>
        /// Gets or sets the name of this release.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// (Required.) Gets or sets the path to the folder that contains all the asset for the release.
        /// </summary>
        public string ReleaseAssetDir { get; set; }

        /// <summary>
        /// Gets or sets the name of the GitHub repository to which to post the new relase.
        /// </summary>
        public string RepoName { get; set; }

        /// <summary>
        /// Gets or sets the GitHub username of the repository to which the release is being posted.
        /// </summary>
        public string RepoOwner { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether a version message should be displayed.
        /// </summary>
        public bool ShouldDisplayVersion { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether the release assets should be compressed into a ZIP file.
        /// </summary>
        /// <remarks>If this flag is true, do not compress the assets.  Otherwise, compress them in a ZIP format file before uploading to the release.
        /// The user must specify the '--no-zip' flag on the command line to set this flag to true.  By default, it's initialized to have the value of false.</remarks>
        public bool ShouldNotZip { get; set; }

        /// <summary>
        /// Gets or sets the name of this release's GitHub tag.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// (Required.) Gets or sets the name of the target branch.  Default is 'master'.
        /// </summary>
        public string TargetBranch { get; set; }

        /// <summary>
        /// Gets or sets the user access token to be passed to GitHub for authentication.
        /// </summary>
        public string UserAccessToken { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the command-line arguments were parsed correctly.
        /// </summary>
        public bool WasCommandLineParsed { get; private set; }

        /// <summary>
        /// Resets this object's properties to their default values.
        /// </summary>
        public void Clear()
        {
            WasCommandLineParsed = false;

            Body = string.Empty;
            IsDraft = false;
            IsPreRelease = false;
            Name = string.Empty;
            ReleaseAssetDir = string.Empty;
            TagName = string.Empty;
            TargetBranch = Resources.MASTER_BRANCH;
            UserAccessToken = string.Empty;
            RepoName = string.Empty;
            RepoOwner = string.Empty;
            ShouldNotZip = false;   /* zip the release assets prior to uploading by default */
        }

        /// <summary>
        /// Parses this application's command line and sets the properties of this object accordingly.
        /// </summary>
        /// <param name="args">Reference to an instance of an array of strings that contains the application's command-line arguments.</param>
        public bool ParseCommandLine(string[] args)
        {
            // If no args were passed, then stop.
            if (!args.Any())
            {
                Program.PrintUsageMessage();
                return false;
            }

            // --version - Displays this application's version.  Must be the only switch specified on the command line.
            if (args.Length == 1
                && args.Contains("--version"))
            {
                ShouldDisplayVersion = true;
                return true;
            }

            var isError = false;

            /* If the user specifies the --no-zip flag on the command line, set the ShouldNotZip property to TRUE */
            ShouldNotZip = args.Contains(Resources.NO_ZIP_SWITCH);

            Body = GetSwitchValue(args, Resources.BODY_SWITCH);
            if (!isError && !string.IsNullOrWhiteSpace(Body))
            {
                Console.WriteLine(Resources.UsingReleaseBody,
                    Body.Replace("\n", string.Empty).Replace("\t", string.Empty));
            }

            Name = GetSwitchValue(args, Resources.NAME_SWITCH);
            if (!isError && !string.IsNullOrWhiteSpace(Name))
            {
                Console.WriteLine(Resources.UsingReleaseName, Name);
            }

            ReleaseAssetDir = GetSwitchValue(args, Resources.RELEASE_ASSET_DIR_SWITCH);
            if (string.IsNullOrWhiteSpace(ReleaseAssetDir))
            {
                Console.WriteLine(Resources.SwitchRequired, Resources.RELEASE_ASSET_DIR_SWITCH);
                isError = true;
            }
            else if (!isError)
            {
                ReleaseAssetDir = Path.GetFullPath(ReleaseAssetDir);
                if (!string.IsNullOrWhiteSpace(ReleaseAssetDir))
                {
                    Console.WriteLine(Resources.ReleaseAssetsComeFromFolder, ReleaseAssetDir);
                }
                else
                {
                    Console.WriteLine(Resources.SwitchRequired, Resources.RELEASE_ASSET_DIR_SWITCH);
                    isError = true;
                }
            }

            // If a release asset directory path was provided, then check that the
            // directory specified exists
            if (!string.IsNullOrWhiteSpace(ReleaseAssetDir)
                && !Directory.Exists(ReleaseAssetDir))
            {
                Console.WriteLine(Resources.ReleaseAssetDirNotFound, ReleaseAssetDir);
                isError = true;
            }
            else if (!isError)
            {
                Console.WriteLine(Resources.LocatedReleaseAssetDirSuccessfully, ReleaseAssetDir);
            }

            TagName = GetSwitchValue(args, Resources.TAG_NAME_SWITCH);
            if (string.IsNullOrWhiteSpace(TagName))
            {
                Console.WriteLine(Resources.SwitchRequired, Resources.TAG_NAME_SWITCH);
                isError = true;
            }
            else if (!isError)
            {
                Console.WriteLine(Resources.UsingTagName, TagName);
            }

            TargetBranch = GetSwitchValue(args, Resources.TARGET_BRANCH_SWITCH, TargetBranch);
            if (string.IsNullOrWhiteSpace(TargetBranch))
            {
                Console.WriteLine(Resources.SwitchRequired, Resources.TARGET_BRANCH_SWITCH);
                Console.WriteLine(Resources.DefaultTargetBranchIsMaster);
                isError = true;
            }
            else if (!isError)
            {
                Console.WriteLine(Resources.UsingTargetBranch, TargetBranch);
            }

            UserAccessToken = GetSwitchValue(args, Resources.USER_ACCESS_TOKEN_SWITCH);
            if (string.IsNullOrWhiteSpace(UserAccessToken))
            {
                Console.WriteLine(Resources.SwitchRequired, Resources.USER_ACCESS_TOKEN_SWITCH);
                isError = true;
            }
            else
            {
                Console.WriteLine(Resources.UsingUserAccessToken, UserAccessToken);
            }

            RepoName = GetSwitchValue(args, Resources.REPO_NAME_SWITCH);
            if (string.IsNullOrWhiteSpace(RepoName))
            {
                Console.WriteLine(Resources.SwitchRequired, Resources.REPO_NAME_SWITCH);
                isError = true;
            }
            else
            {
                Console.WriteLine(Resources.UsingRepoName, RepoName);
            }

            RepoOwner = GetSwitchValue(args, Resources.REPO_OWNER_SWITCH);
            if (string.IsNullOrWhiteSpace(RepoOwner))
            {
                Console.WriteLine(Resources.SwitchRequired, Resources.REPO_OWNER_SWITCH);
                isError = true;
            }
            else
            {
                Console.WriteLine(Resources.UsingRepoOwner, RepoOwner);
            }

            IsDraft = args.Contains(Resources.IS_DRAFT_SWITCH);

            IsPreRelease = args.Contains(Resources.IS_PRE_RELEASE_SWITCH);

            if (isError)
                Environment.Exit(Resources.FAILED_TO_PARSE_COMMAND_LINE);

            WasCommandLineParsed = !isError;
            return WasCommandLineParsed;
        }

        /// <summary>
        /// Gets the value of a command-line switch that has a parameter value following it.
        /// </summary>
        /// <param name="args">Reference to a collection of the command-line arguments passed to this application.</param>
        /// <param name="switchName">String containing the name of the switch (required.)</param>
        /// <param name="defaultValue">Default value, if applicable, to use if the switch is not supplied on the command-line.</param>
        /// <returns></returns>
        private static string GetSwitchValue(IReadOnlyList<string> args, string switchName, string defaultValue = "")
        {
            if (!args.Any())
                return string.Empty;

            if (string.IsNullOrWhiteSpace(switchName))
                return string.Empty;

            try
            {
                var switchIndex = args.ToList().FindIndex(switchName.Equals);
                return switchIndex >= 0 && !args[switchIndex + 1].StartsWith("--") ? args[switchIndex + 1] : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
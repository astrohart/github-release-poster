﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace github_release_poster.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("github_release_poster.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Accept.
        /// </summary>
        public static string AcceptHeaderName {
            get {
                return ResourceManager.GetString("AcceptHeaderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NOTE: All switches are case-sensitive.  The contents of the switches aren&apos;t unless otherwise noted..
        /// </summary>
        public static string AllSwitchesCaseSensitive {
            get {
                return ResourceManager.GetString("AllSwitchesCaseSensitive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authorization.
        /// </summary>
        public static string AuthorizationHeaderName {
            get {
                return ResourceManager.GetString("AuthorizationHeaderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --body.
        /// </summary>
        public static string BODY_SWITCH {
            get {
                return ResourceManager.GetString("BODY_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --body: Text body for the release (optional)..
        /// </summary>
        public static string BODY_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("BODY_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to cache-control.
        /// </summary>
        public static string CacheControlHeaderName {
            get {
                return ResourceManager.GetString("CacheControlHeaderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.github.com/repos/astrohart/NetworkKeeper/releases.
        /// </summary>
        public static string CreateReleaseApiPostURL {
            get {
                return ResourceManager.GetString("CreateReleaseApiPostURL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Default target branch is &apos;master&apos;..
        /// </summary>
        public static string DefaultTargetBranchIsMaster {
            get {
                return ResourceManager.GetString("DefaultTargetBranchIsMaster", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0}\n\t{1}.
        /// </summary>
        public static string ExceptionMessageFormat {
            get {
                return ResourceManager.GetString("ExceptionMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to 0.
        /// </summary>
        public static int EXIT_SUCCESS {
            get {
                object obj = ResourceManager.GetObject("EXIT_SUCCESS", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -103.
        /// </summary>
        public static int FAILED_TO_INITIALIZE_LOGGING {
            get {
                object obj = ResourceManager.GetObject("FAILED_TO_INITIALIZE_LOGGING", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -101.
        /// </summary>
        public static int FAILED_TO_PARSE_COMMAND_LINE {
            get {
                object obj = ResourceManager.GetObject("FAILED_TO_PARSE_COMMAND_LINE", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -102.
        /// </summary>
        public static int FAILED_TO_PROCESS_COMMAND_LINE {
            get {
                object obj = ResourceManager.GetObject("FAILED_TO_PROCESS_COMMAND_LINE", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: Failed to format the JSON of the create-release request body..
        /// </summary>
        public static string FailedToFormatReleaseJson {
            get {
                return ResourceManager.GetString("FailedToFormatReleaseJson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: Failed to initialize log file..
        /// </summary>
        public static string FailedToInitializeLogFile {
            get {
                return ResourceManager.GetString("FailedToInitializeLogFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: Could not package release for posting..
        /// </summary>
        public static string FailedToPackageReleaseForPosting {
            get {
                return ResourceManager.GetString("FailedToPackageReleaseForPosting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to validate release metadata..
        /// </summary>
        public static string FailedValidateReleaseMetadata {
            get {
                return ResourceManager.GetString("FailedValidateReleaseMetadata", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to application/vnd.github.v3.raw.
        /// </summary>
        public static string GitHubApiV3Accept {
            get {
                return ResourceManager.GetString("GitHubApiV3Accept", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --is-draft.
        /// </summary>
        public static string IS_DRAFT_SWITCH {
            get {
                return ResourceManager.GetString("IS_DRAFT_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --is-draft: Specify this switch to mark this release as a draft.  Leave this switch off to not do so..
        /// </summary>
        public static string IS_DRAFT_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("IS_DRAFT_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --is-pre-release.
        /// </summary>
        public static string IS_PRE_RELEASE_SWITCH {
            get {
                return ResourceManager.GetString("IS_PRE_RELEASE_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --is-pre-release: Specify this switch to mark this release as pre-release; leave it off to not do so..
        /// </summary>
        public static string IS_PRE_RELEASE_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("IS_PRE_RELEASE_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The release asset folder &apos;{0}&apos; has been located..
        /// </summary>
        public static string LocatedReleaseAssetDirSuccessfully {
            get {
                return ResourceManager.GetString("LocatedReleaseAssetDirSuccessfully", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to master.
        /// </summary>
        public static string MASTER_BRANCH {
            get {
                return ResourceManager.GetString("MASTER_BRANCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --name.
        /// </summary>
        public static string NAME_SWITCH {
            get {
                return ResourceManager.GetString("NAME_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --name: String containing the name of this release.  Surround with quotes if it contains spaces..
        /// </summary>
        public static string NAME_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("NAME_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --no-zip.
        /// </summary>
        public static string NO_ZIP_SWITCH {
            get {
                return ResourceManager.GetString("NO_ZIP_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --no-zip: Specify this flag on the command line to turn off the automatic ZIPping of release assets..
        /// </summary>
        public static string NO_ZIP_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("NO_ZIP_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to no-cache.
        /// </summary>
        public static string NoCacheHeader {
            get {
                return ResourceManager.GetString("NoCacheHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: No output path specified for the release asset ZIP file..
        /// </summary>
        public static string OutputZipFilePathBlank {
            get {
                return ResourceManager.GetString("OutputZipFilePathBlank", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Posting release &apos;{0}&apos; to repository &apos;{1}&apos;....
        /// </summary>
        public static string PostingReleaseToWhichRepo {
            get {
                return ResourceManager.GetString("PostingReleaseToWhichRepo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Processing release assets....
        /// </summary>
        public static string ProcessingReleaseAssets {
            get {
                return ResourceManager.GetString("ProcessingReleaseAssets", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to github-release-poster.
        /// </summary>
        public static string PROGRAM_NAME {
            get {
                return ResourceManager.GetString("PROGRAM_NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --release-asset-dir.
        /// </summary>
        public static string RELEASE_ASSET_DIR_SWITCH {
            get {
                return ResourceManager.GetString("RELEASE_ASSET_DIR_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --release-asset-dir: Path to directory containing the assets for this release..
        /// </summary>
        public static string RELEASE_ASSET_DIR_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("RELEASE_ASSET_DIR_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The release asset directory &apos;{0}&apos; could not be located.  Please check whether the directory exists..
        /// </summary>
        public static string ReleaseAssetDirNotFound {
            get {
                return ResourceManager.GetString("ReleaseAssetDirNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Release assets come from folder &apos;{0}&apos;..
        /// </summary>
        public static string ReleaseAssetsComeFromFolder {
            get {
                return ResourceManager.GetString("ReleaseAssetsComeFromFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --repo-name.
        /// </summary>
        public static string REPO_NAME_SWITCH {
            get {
                return ResourceManager.GetString("REPO_NAME_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --repo-name &lt;name&gt;: Name of the GitHub repository to which the release should be posted..
        /// </summary>
        public static string REPO_NAME_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("REPO_NAME_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --repo-owner.
        /// </summary>
        public static string REPO_OWNER_SWITCH {
            get {
                return ResourceManager.GetString("REPO_OWNER_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --repo-owner: GitHub username of the owner of the repository to which the release should be posted..
        /// </summary>
        public static string REPO_OWNER_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("REPO_OWNER_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sending HTTP POST request to create release &apos;{0}&apos;....
        /// </summary>
        public static string SendingPostRequestToCreateRelease {
            get {
                return ResourceManager.GetString("SendingPostRequestToCreateRelease", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The switch &apos;{0}&apos; is required..
        /// </summary>
        public static string SwitchRequired {
            get {
                return ResourceManager.GetString("SwitchRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --tag-name.
        /// </summary>
        public static string TAG_NAME_SWITCH {
            get {
                return ResourceManager.GetString("TAG_NAME_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --tag-name: GitHub tag to associate this release with..
        /// </summary>
        public static string TAG_NAME_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("TAG_NAME_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --target-branch.
        /// </summary>
        public static string TARGET_BRANCH_SWITCH {
            get {
                return ResourceManager.GetString("TARGET_BRANCH_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --target-branch: GitHub branch to target this release from..
        /// </summary>
        public static string TARGET_BRANCH_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("TARGET_BRANCH_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to test app.
        /// </summary>
        public static string TestAppUserAgent {
            get {
                return ResourceManager.GetString("TestAppUserAgent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to undefined.
        /// </summary>
        public static string UndefinedParameterName {
            get {
                return ResourceManager.GetString("UndefinedParameterName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage:.
        /// </summary>
        public static string Usage {
            get {
                return ResourceManager.GetString("Usage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --user-access-token.
        /// </summary>
        public static string USER_ACCESS_TOKEN_SWITCH {
            get {
                return ResourceManager.GetString("USER_ACCESS_TOKEN_SWITCH", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --user-access-token: String specifying the user access token to use with GitHub API..
        /// </summary>
        public static string USER_ACCESS_TOKEN_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("USER_ACCESS_TOKEN_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User-Agent.
        /// </summary>
        public static string UserAgentHeaderName {
            get {
                return ResourceManager.GetString("UserAgentHeaderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: Current user does not have access permissions to folder &apos;{0}&apos;..
        /// </summary>
        public static string UserNotHasPermissionsToFolder {
            get {
                return ResourceManager.GetString("UserNotHasPermissionsToFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Release body is:
        ///{0}.
        /// </summary>
        public static string UsingReleaseBody {
            get {
                return ResourceManager.GetString("UsingReleaseBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Release name is &apos;{0}&apos;.
        /// </summary>
        public static string UsingReleaseName {
            get {
                return ResourceManager.GetString("UsingReleaseName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Using repo-name &apos;{0}&apos;..
        /// </summary>
        public static string UsingRepoName {
            get {
                return ResourceManager.GetString("UsingRepoName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Using repo-owner &apos;{0}&apos;..
        /// </summary>
        public static string UsingRepoOwner {
            get {
                return ResourceManager.GetString("UsingRepoOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Using release tag &apos;{0}&apos;..
        /// </summary>
        public static string UsingTagName {
            get {
                return ResourceManager.GetString("UsingTagName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Using target branch &apos;{0}&apos;..
        /// </summary>
        public static string UsingTargetBranch {
            get {
                return ResourceManager.GetString("UsingTargetBranch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Using user access token &apos;{0}&apos;..
        /// </summary>
        public static string UsingUserAccessToken {
            get {
                return ResourceManager.GetString("UsingUserAccessToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to --version: This switch displays this program&apos;s name and version number, along with a copyright message.  If specified, the --version switch must appear by itself..
        /// </summary>
        public static string VERSION_SWITCH_USAGE {
            get {
                return ResourceManager.GetString("VERSION_SWITCH_USAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} version {1}.
        /// </summary>
        public static string VersionString {
            get {
                return ResourceManager.GetString("VersionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: The output ZIP file for the release assets may not be in the same folder as the assets themselves..
        /// </summary>
        public static string ZipFolderMustBeDifferentFromAssetFolder {
            get {
                return ResourceManager.GetString("ZipFolderMustBeDifferentFromAssetFolder", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: Blank value supplied for ZIP file output folder path. Stopping..
        /// </summary>
        public static string ZipOutputFolderBlank {
            get {
                return ResourceManager.GetString("ZipOutputFolderBlank", resourceCulture);
            }
        }
    }
}

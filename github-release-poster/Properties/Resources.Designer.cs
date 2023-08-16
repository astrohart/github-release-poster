//------------------------------------------------------------------------------
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
        ///   Looks up a localized string similar to application/octet-stream.
        /// </summary>
        public static string ApplicationOctetStreamMimeType {
            get {
                return ResourceManager.GetString("ApplicationOctetStreamMimeType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The asset &apos;{0}&apos; has been accepted by GitHub..
        /// </summary>
        public static string AssetAccepted {
            get {
                return ResourceManager.GetString("AssetAccepted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to assets.zip.
        /// </summary>
        public static string AssetsZipName {
            get {
                return ResourceManager.GetString("AssetsZipName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to $The asset compression process has resulted in a package that has zero size..
        /// </summary>
        public static string AssetZipProcessResultedInZeroSizePackage {
            get {
                return ResourceManager.GetString("AssetZipProcessResultedInZeroSizePackage", resourceCulture);
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
        ///   Looks up a localized string similar to Content-Type.
        /// </summary>
        public static string ContentTypeHeaderName {
            get {
                return ResourceManager.GetString("ContentTypeHeaderName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.github.com/repos/{0}/{1}/releases.
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
        ///   Looks up a localized string similar to The directory &apos;{0}&apos; could not be located..
        /// </summary>
        public static string DirectoryNotFound {
            get {
                return ResourceManager.GetString("DirectoryNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -108.
        /// </summary>
        public static int ERROR_ASSET_NOT_ACCEPTED {
            get {
                object obj = ResourceManager.GetObject("ERROR_ASSET_NOT_ACCEPTED", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -107.
        /// </summary>
        public static int ERROR_FAILED_TO_ZIP_ASSETS {
            get {
                object obj = ResourceManager.GetObject("ERROR_FAILED_TO_ZIP_ASSETS", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -106.
        /// </summary>
        public static int ERROR_NOT_OBTAINED_RELEASE_UPLOAD_URL {
            get {
                object obj = ResourceManager.GetObject("ERROR_NOT_OBTAINED_RELEASE_UPLOAD_URL", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -104.
        /// </summary>
        public static int ERROR_RELEASE_ASSET_DIR_NOT_EXISTS {
            get {
                object obj = ResourceManager.GetObject("ERROR_RELEASE_ASSET_DIR_NOT_EXISTS", resourceCulture);
                return ((int)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Int32 similar to -105.
        /// </summary>
        public static int ERROR_RELEASE_ASSET_IS_TOO_BIG {
            get {
                object obj = ResourceManager.GetObject("ERROR_RELEASE_ASSET_IS_TOO_BIG", resourceCulture);
                return ((int)(obj));
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
        ///   Looks up a localized string similar to Failed to upload asset &apos;{0}&apos;..
        /// </summary>
        public static string FailedToUploadAsset {
            get {
                return ResourceManager.GetString("FailedToUploadAsset", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: Unable to zip the assets in folder &apos;{0}&apos;..
        /// </summary>
        public static string FailedToZipAssets {
            get {
                return ResourceManager.GetString("FailedToZipAssets", resourceCulture);
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
        ///   Looks up a localized string similar to token {0}.
        /// </summary>
        public static string GitHubAuthorizationHeaderContent {
            get {
                return ResourceManager.GetString("GitHubAuthorizationHeaderContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0.7.3.
        /// </summary>
        public static string GitHubIndivAssetTestingReleaseName {
            get {
                return ResourceManager.GetString("GitHubIndivAssetTestingReleaseName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to github-release-poster by xyLOGIX.
        /// </summary>
        public static string GitHubReleasePosterUserAgent {
            get {
                return ResourceManager.GetString("GitHubReleasePosterUserAgent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 0.7.4.
        /// </summary>
        public static string GitHubZipAssetsTestingReleaseName {
            get {
                return ResourceManager.GetString("GitHubZipAssetsTestingReleaseName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \{\?([^}]+)\}.
        /// </summary>
        public static string HypermediaRelationUriTemplateRegex {
            get {
                return ResourceManager.GetString("HypermediaRelationUriTemplateRegex", resourceCulture);
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
        ///   Looks up a localized string similar to master.
        /// </summary>
        public static string MasterBranchName {
            get {
                return ResourceManager.GetString("MasterBranchName", resourceCulture);
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
        ///   Looks up a localized string similar to ERROR: No files found in the destination that were suitable for ZIPping..
        /// </summary>
        public static string NoFilesFoundForZipping {
            get {
                return ResourceManager.GetString("NoFilesFoundForZipping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This release&apos;s assets will NOT be zipped up prior to upload..
        /// </summary>
        public static string NotZippingReleaseAssets {
            get {
                return ResourceManager.GetString("NotZippingReleaseAssets", resourceCulture);
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
        ///   Looks up a localized string similar to The GitHubReleasePoster.PostNewRelease method did not succeed..
        /// </summary>
        public static string PostReleaseTestNotSucceeded {
            get {
                return ResourceManager.GetString("PostReleaseTestNotSucceeded", resourceCulture);
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
        ///   Looks up a localized string similar to ERROR: The release asset directory path, &apos;{0}&apos;, contains characters that Windows does not allow to be present in a file or directory name..
        /// </summary>
        public static string ReleaseAssetDirContainsInvalidChars {
            get {
                return ResourceManager.GetString("ReleaseAssetDirContainsInvalidChars", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: The release asset directory or one of its subfolders contains a file that is 2 GB or greater in size..
        /// </summary>
        public static string ReleaseAssetDirContainsTooBigFile {
            get {
                return ResourceManager.GetString("ReleaseAssetDirContainsTooBigFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: The release asset directory &apos;{0}&apos; could not be located.  Please check whether the directory exists..
        /// </summary>
        public static string ReleaseAssetDirNotFound {
            get {
                return ResourceManager.GetString("ReleaseAssetDirNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The release metadata and assets are valid..
        /// </summary>
        public static string ReleaseAssetsAndMetadataValid {
            get {
                return ResourceManager.GetString("ReleaseAssetsAndMetadataValid", resourceCulture);
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
        ///   Looks up a localized string similar to Release &apos;{0}&apos; and its assets have been posted to GitHub..
        /// </summary>
        public static string ReleasePostedToGitHub {
            get {
                return ResourceManager.GetString("ReleasePostedToGitHub", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This release is being marked as Draft..
        /// </summary>
        public static string ReleaseWillBeMarkedDraft {
            get {
                return ResourceManager.GetString("ReleaseWillBeMarkedDraft", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This release is being marked as Pre-Release..
        /// </summary>
        public static string ReleaseWillBeMarkedPreRelease {
            get {
                return ResourceManager.GetString("ReleaseWillBeMarkedPreRelease", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This release is NOT being marked as Draft..
        /// </summary>
        public static string ReleaseWillNotBeMarkedDraft {
            get {
                return ResourceManager.GetString("ReleaseWillNotBeMarkedDraft", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This release is NOT being marked as Pre-Release..
        /// </summary>
        public static string ReleaseWillNotBeMarkedPreRelease {
            get {
                return ResourceManager.GetString("ReleaseWillNotBeMarkedPreRelease", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: The name of the destination ZIP file &apos;{0}&apos;  for the release assets contains invalid characters..
        /// </summary>
        public static string ReleaseZipFileNameContainsInvalidChars {
            get {
                return ResourceManager.GetString("ReleaseZipFileNameContainsInvalidChars", resourceCulture);
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
        ///   Looks up a localized string similar to C:\Users\ENS Brian Hart\Dropbox\Downloads\emu8086.
        /// </summary>
        public static string TestingAssetsSourceDirPath {
            get {
                return ResourceManager.GetString("TestingAssetsSourceDirPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NetworkKeeper.
        /// </summary>
        public static string TestingRepoName {
            get {
                return ResourceManager.GetString("TestingRepoName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to astrohart.
        /// </summary>
        public static string TestingRepoOwner {
            get {
                return ResourceManager.GetString("TestingRepoOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.UInt64 similar to 2147483648.
        /// </summary>
        public static ulong TwoGigaBytes {
            get {
                object obj = ResourceManager.GetObject("TwoGigaBytes", resourceCulture);
                return ((ulong)(obj));
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
        ///   Looks up a localized string similar to {0}?name={1}&amp;label={2}.
        /// </summary>
        public static string UploadAssetURL {
            get {
                return ResourceManager.GetString("UploadAssetURL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ERROR: Unable to get upload_url value from create release response..
        /// </summary>
        public static string UploadUrlNotObtainable {
            get {
                return ResourceManager.GetString("UploadUrlNotObtainable", resourceCulture);
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
        ///   Looks up a localized string similar to Validating release metadata....
        /// </summary>
        public static string ValidatingReleaseMetadata {
            get {
                return ResourceManager.GetString("ValidatingReleaseMetadata", resourceCulture);
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
        ///   Looks up a localized string similar to application/zip.
        /// </summary>
        public static string ZipFileContentType {
            get {
                return ResourceManager.GetString("ZipFileContentType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The size of the &apos;{0}&apos; file is 2 GB or more in size.  GitHub won&apos;t accept it..
        /// </summary>
        public static string ZipFileTooBig {
            get {
                return ResourceManager.GetString("ZipFileTooBig", resourceCulture);
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
        
        /// <summary>
        ///   Looks up a localized string similar to This release&apos;s assets will be placed in a ZIP file prior to upload..
        /// </summary>
        public static string ZippingReleaseAssets {
            get {
                return ResourceManager.GetString("ZippingReleaseAssets", resourceCulture);
            }
        }
    }
}

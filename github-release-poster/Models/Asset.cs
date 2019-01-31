using GitHubReleasePoster.Interfaces;

namespace GitHubReleasePoster.Models
{
    /// <summary>
    /// Model for an individual asset in a release.
    /// </summary>
    internal class Asset : IAsset
    {
        /// <summary>
        /// The name for the asset that should appear on the GitHub website.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The name of the asset's filename that shows when the user clicks the asset's link.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Local pathname of the asset's file on the local computer.
        /// </summary>
        public string Path { get; set; }
    }
}
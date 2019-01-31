using System.Collections.Generic;

namespace GitHubReleasePoster.Interfaces
{
    /// <summary>
    /// Release to be posted to GitHub.
    /// </summary>
    public interface IRelease
    {
        /// <summary>
        /// Gets or sets a reference to a list of instances of
        /// <see cref="T:GitHubReleasePoster.Models.Asset"/> that
        /// refer to the assets in this release.
        /// </summary>
        List<IAsset> Assets { get; set; }

        /// <summary>
        /// Gets or sets the ID for the release (as stated by the GitHub API).
        /// </summary>
        string Id { get; set; }
    }
}
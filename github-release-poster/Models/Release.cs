using System.Collections.Generic;
using GitHubReleasePoster.Interfaces;

namespace GitHubReleasePoster.Models
{
    /// <summary>
    /// Model for a GitHub release.
    /// </summary>
    internal class Release : IRelease
    {
        ///<summary>
        /// Constructs a new instance of <see cref="T:GitHubReleasePoster.Models.Release"/>
        /// and returns a reference to it.
        ///</summary>
        public Release()
        {
            Assets = new List<IAsset>();
        }

        /// <summary>
        /// Gets or sets a reference to a list of instances of
        /// <see cref="T:GitHubReleasePoster.Models.Asset"/> that
        /// refer to the assets in this release.
        /// </summary>
        public List<IAsset> Assets { get; set; }

        /// <summary>
        /// Gets or sets the ID for the release (as stated by the GitHub API).
        /// </summary>
        public string Id { get; set; }
    }
}
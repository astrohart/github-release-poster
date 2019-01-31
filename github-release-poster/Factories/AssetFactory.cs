using GitHubReleasePoster.Interfaces;
using GitHubReleasePoster.Models;

namespace GitHubReleasePoster.Factories
{
    /// <summary>
    /// Creates new instances of objects that implement
    /// <see cref="T:GitHubReleasePoster.Interfaces.IAsset"/>.
    /// </summary>
    public static class AssetFactory
    {
        /// <summary>
        /// Creates a new instance of an object implementing the
        /// <see cref="T:GitHubReleasePoster.Interfaces.IAsset"/>
        /// interface.
        /// </summary>
        /// <param name="label">Label to use for the release asset (displayed on the GitHub website).</param>
        /// <param name="name">Filename for the browser download.</param>
        /// <param name="path">Local path of the file to upload for the asset.</param>
        /// <returns>Reference to a new instance of an object that implements
        /// <see cref="T:GitHubReleasePoster.Interfaces.IAsset"/> whose properties
        /// are initialized with the asset's data.</returns>
        public static IAsset MakeNewAsset(string label, string name, string path)
        {
            return new Asset
            {
                Label = label,
                Name = name,
                Path = path
            };
        }
    }
}
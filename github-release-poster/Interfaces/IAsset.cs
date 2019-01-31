namespace GitHubReleasePoster.Interfaces
{
    /// <summary>
    /// Asset to be part of a GitHub release.
    /// </summary>
    public interface IAsset
    {
        /// <summary>
        /// The name for the asset that should appear on the GitHub website.
        /// </summary>
        string Label { get; set; }

        /// <summary>
        /// The name of the asset's filename that shows when the user clicks the asset's link.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Local pathname of the asset's file on the local computer.
        /// </summary>
        string Path { get; set; }
    }
}
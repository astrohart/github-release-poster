namespace github_release_poster
{
    public class NewRelease
    {
        public string body { get; set; }
        public bool draft { get; set; }
        public string name { get; set; }
        public bool prerelease { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
    }
}
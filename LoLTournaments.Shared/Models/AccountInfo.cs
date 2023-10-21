namespace LoLTournaments.Shared.Models
{

    public struct AccountInfo
    {
        public string Id { get; set; }
        public string IconUrl { get; set; }
        public string Region { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Tier { get; set; }

        public bool Available => !string.IsNullOrEmpty(Id);
        public bool Known { get; set; }
    }

}
namespace LoLTournaments.Shared.Models
{
    public struct MemberStat
    {
        public int Win { get; set; }
        public int Lose { get; set; }

        public override string ToString()
        {
            return $"{Win}-{Lose}";
        }
    }
}
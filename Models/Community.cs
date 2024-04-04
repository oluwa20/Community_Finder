namespace Community_Finder2.Models
{
    public class Community
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}

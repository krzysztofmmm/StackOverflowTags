namespace StackOverflowTags.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
        public bool HasSynonyms { get; set; }
        public bool IsModeratorOnly { get; set; }
        public bool IsRequired { get; set; }
    }
}

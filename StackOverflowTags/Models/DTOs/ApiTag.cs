namespace StackOverflowTags.Models.DTOs
{
    public class ApiTag
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public bool HasSynonyms { get; set; }
        public bool IsModeratorOnly { get; set; }
        public bool IsRequired { get; set; }
    }
}

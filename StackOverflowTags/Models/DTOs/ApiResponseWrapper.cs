using StackOverflowTags.Models.DTOs;

namespace StackOverflowTags.DTOs
{
    public class ApiResponseWrapper
    {
        public List<ApiTag> Items { get; set; }
        public bool HasMore { get; set; }
        public int QuotaMax { get; set; }
        public int QuotaRemaining { get; set; }
    }
}

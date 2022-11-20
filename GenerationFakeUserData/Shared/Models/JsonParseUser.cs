namespace GenerationFakeUserData.Shared.Models
{
    public class JsonParseUser
    {
        public UserFIO Male { get; set; } = default!;
        public UserFIO Female { get; set; } = default!;
        public List<string> City { get; set; } = default!;
        public List<string> Street { get; set; } = default!;
        public List<string> Village { get; set; } = default!;
        public List<string> Region { get; set; } = default!;
        public Phone Phone { get; set; } = default!;
        public string Letter { get; set; } = default!;
    }
}

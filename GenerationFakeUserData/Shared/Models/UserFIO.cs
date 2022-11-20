namespace GenerationFakeUserData.Shared.Models
{
    public class UserFIO
    {
        public string[] FirstName { get; set; } = default!;
        public string[]? MiddleName { get; set; }
        public string[] LastName { get; set; } = default!;
    }
}

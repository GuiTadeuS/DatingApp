namespace API.Entities
{
    public class UserLike
    {
        public AppUser SourceUser { get; set; } // Navigation Property

        public int SourceUserId { get; set; }

        public AppUser TargetUser { get; set; } // Navigation Property

        public int TargetUserId { get; set; }
    }
}

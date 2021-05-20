namespace API.Entities
{
    public class LikeIt
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public AppUser User { get; set; }
        public Post Post { get; set; }
    }
}
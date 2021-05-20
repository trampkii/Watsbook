namespace API.Entities
{
    public class Friend
    {
        public int UserId { get; set; }
        public int SecondUserId { get; set; }
        public AppUser User { get; set; }
        public AppUser SecondUser { get; set; }
    }
}
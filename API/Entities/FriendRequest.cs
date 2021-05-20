namespace API.Entities
{
    public class FriendRequest
    {
        public int SenderId { get; set; }
        public AppUser Sender { get; set; }
        public int RecipientId { get; set; }
        public AppUser Recipient { get; set; }
    }
}
namespace API.DataTransferObjects
{
    public class DetailedUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; } 
        public string Location { get; set; }
        public string PhotoUrl { get; set; }
        
    }
}
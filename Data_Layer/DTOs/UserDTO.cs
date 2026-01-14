namespace DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string password { get; set; } 
        public byte[]? Picture { get; set; } 
        public bool Admin { get; set; }

    }
}


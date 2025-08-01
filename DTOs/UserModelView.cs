using Task4.Migrations;

namespace Task4.DTOs
{
    public class UserModelView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool isActive { get; set; }
        public DateTime LastSeen { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Task4.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public bool isActive { get; set; }
        public DateTime LastSeen { get; set; }
        public virtual RoleModel Role { get; set; }
    }
}
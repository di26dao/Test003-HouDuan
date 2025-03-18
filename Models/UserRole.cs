using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Test003.Models
{
    public class UserRole
    {
        [Key]
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
    }
}

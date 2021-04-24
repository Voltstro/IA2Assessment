using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IA2Assessment.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
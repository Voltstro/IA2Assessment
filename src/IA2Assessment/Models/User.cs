using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace IA2Assessment.Models
{
    [Table("Users")]
    public partial class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(128)]
        public string UserFirstName { get; set; }
        
        [Required]
        [StringLength(128)]
        public string UserLastName { get; set; }
        
        [Required]
        [StringLength(256)]
        public string UserPasswordHash { get; set; }

        [StringLength(256)]
        public string UserAllergies { get; set; }
        
        [Column(TypeName = "decimal(10,0)")]
        public decimal? UserPrepaidBalance { get; set; }
    }

    public enum UserLevel
    {
        Admin = 0,
        TuckshopStaff,
        Staff,
        Grade7,
        Grade8,
        Grade9,
        Grade10,
        Grade11,
        Grade12,
    }
}

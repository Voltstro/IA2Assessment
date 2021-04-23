using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace IA2Assessment.Models
{
    public partial class User
    {
        [Key]
        [StringLength(256)]
        public string UserLogin { get; set; }
        [Required]
        [StringLength(256)]
        public string UserPasswordHash { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        [Required]
        [StringLength(16)]
        public string UserLevel { get; set; }
        [StringLength(256)]
        public string UserAllergies { get; set; }
        [Column(TypeName = "decimal(10,0)")]
        public decimal? UserPrepaidBalance { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace IA2Assessment.Models
{
    public partial class MenuItem
    {
        [Key]
        [Column("ItemID")]
        public int ItemId { get; set; }
        
        [Required]
        [StringLength(256)]
        public string ItemName { get; set; }
        
        [Column(TypeName = "decimal(10,0)")]
        public decimal ItemPrice { get; set; }
        public int? ItemAvailabilityQuantity { get; set; }
        
        [Required]
        [StringLength(256)]
        public string ItemNutrientInfo { get; set; }
        
        [Required]
        [StringLength(128)]
        public string ItemDisplayUrl { get; set; }

        [NotMapped] public int ItemBoughtCount { get; set; } = 1;
    }
}

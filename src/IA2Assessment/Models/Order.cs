using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace IA2Assessment.Models
{
    public partial class Order
    {
        [Key]
        [Column("OrderID")]
        public int OrderId { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; }
        
        [Required]
        public TimeSpan OrderTime { get; set; }
        
        [Required]
        public OrderStatus OrderStatus { get; set; }
        
        [Required]
        [ForeignKey(nameof(User))]
        public int OrderUserId { get; set; }

        public virtual User User { get; set; }
    }

    public enum OrderStatus
    {
        Outstanding,
        Completed
    }
}

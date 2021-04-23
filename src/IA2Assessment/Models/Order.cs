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
        [Column(TypeName = "date")]
        public DateTime OrderDate { get; set; }
        public TimeSpan OrderTime { get; set; }
        public int OrderStatus { get; set; }
        [Required]
        [StringLength(256)]
        public string OrderUser { get; set; }
    }
}

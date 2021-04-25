using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace IA2Assessment.Models
{
    [Index(nameof(OrderDetailId), Name = "OrderDetailID", IsUnique = true)]
    public partial class OrdersDetail
    {
        [Key]
        [Column("OrderDetailID")]
        public int OrderDetailId { get; set; }
        
        [Column("OrderID")]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        
        [Column("ItemID")]
        [ForeignKey(nameof(MenuItem))]
        public int ItemId { get; set; }
        
        public int ItemQuantity { get; set; }
        
        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}

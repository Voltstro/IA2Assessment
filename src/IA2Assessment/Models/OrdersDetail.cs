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
        public int OrderId { get; set; }
        [Column("ItemID")]
        public int ItemId { get; set; }
        public int ItemQuantity { get; set; }
    }
}

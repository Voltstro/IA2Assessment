using System;
using System.Collections.Generic;

namespace IA2Assessment.Models.Views
{
    public class OrdersManageViewModel
    {
        public class OrderView
        {
            public int OrderId { get; set; }
            public int OrderUserId { get; set; }
            public MenuItem[] MenuItems { get; set; }
            public OrderStatus OrderStatus { get; set; }
            public DateTime OrderDate { get; set; }
            public TimeSpan OrderTime { get; set; }
        }
        
        public enum OrderStatus
        {
            Outstanding = 0,
            Completed = 1
        }

        public List<OrderView> OrdersDetails = new List<OrderView>();
    }
}
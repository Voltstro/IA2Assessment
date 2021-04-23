using System;
using System.Collections.Generic;
using System.Linq;
using IA2Assessment.Models;
using IA2Assessment.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace IA2Assessment.Controllers
{
    public class OrdersController : Controller
    {
        private readonly TuckshopDbContext context;
        
        public OrdersController(TuckshopDbContext context)
        {
            this.context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage()
        {
            OrdersManageViewModel model = new OrdersManageViewModel();
            
            List<Order> orders = context.Orders.ToList();
            foreach (Order order in orders)
            {
                OrdersManageViewModel.OrderView orderView = new OrdersManageViewModel.OrderView
                {
                    OrderId = order.OrderId,
                    OrderUser = order.OrderUser,
                    OrderDate = order.OrderDate,
                    OrderTime = order.OrderTime,
                    OrderStatus = (OrdersManageViewModel.OrderStatus) order.OrderStatus
                };
                
                //Get all order details relating to this order
                
                OrdersDetail[] allOrderDetails = context.OrdersDetails.ToArray();
                OrdersDetail[] orderDetails = allOrderDetails.Where(x => x.OrderId == order.OrderId).ToArray();
                List<MenuItem> menuItems = new List<MenuItem>();
                foreach (OrdersDetail orderDetail in orderDetails)
                {
                    MenuItem[] allItems = context.MenuItems.ToArray();
                    MenuItem[] items = allItems.Where(x => x.ItemId == orderDetail.ItemId).ToArray();
                    menuItems.AddRange(items);
                }

                orderView.MenuItems = menuItems.ToArray();
                
                model.OrdersDetails.Add(orderView);
            }
            
            return View("Manage", model);
        }
    }
}
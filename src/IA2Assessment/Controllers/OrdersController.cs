using System.Collections.Generic;
using System.Linq;
using IA2Assessment.Helper;
using IA2Assessment.Models;
using IA2Assessment.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IA2Assessment.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly TuckshopDbContext context;
        
        public OrdersController(TuckshopDbContext context)
        {
            this.context = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            MenuItem[] menuItems = context.MenuItems.ToArray();
            ViewBag.Order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            ViewBag.MenuItems = menuItems;
            return View("Index");
        }
        
        public IActionResult View(OrdersNewOrderModel model)
        {
            ViewBag.Order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            return View("View", model);
        }
        
        [Route("Add/{id:int}")]
        public IActionResult Add(int id)
        {
            MenuItem item = context.MenuItems.FirstOrDefault(x => x.ItemId == id);
            if (item == null)
                return RedirectToAction("View");
            
            if (HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order") == null)
            {
                List<MenuItem> order = new List<MenuItem> {item};
                HttpContext.Session.SetObjectAsJson("Order", order);
            }
            else
            {
                List<MenuItem> order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
                order.Add(item);
                HttpContext.Session.SetObjectAsJson("Order", order);
            }

            return RedirectToAction("View");
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
                    OrderUserId = order.OrderId,
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IA2Assessment.Helper;
using IA2Assessment.Models;
using IA2Assessment.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IA2Assessment.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly TuckshopDbContext context;
        private readonly UserManager<User> userManager;
        
        public OrdersController(TuckshopDbContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            MenuItem[] menuItems = context.MenuItems.ToArray();
            ViewBag.Order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            ViewBag.MenuItems = menuItems;
            return View("Index");
        }
        
        [HttpGet]
        public new IActionResult View()
        {
            List<MenuItem> order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            if (order == null || order.Count == 0)
                return RedirectToAction("Index");

            ViewBag.Order = order;
            
            return View("View");
        }

        [HttpGet]
        public IActionResult Payment()
        {
            List<MenuItem> order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            if (order == null || order.Count == 0)
                return RedirectToAction("Index");
            
            return View("Payment", new OrdersPaymentViewModel
            {
                MenuItems = order.ToArray()
            });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment()
        {
            List<MenuItem> orderItems = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            if (orderItems == null || orderItems.Count == 0)
                return RedirectToAction("Index");

            User user = await userManager.GetUserAsync(User);
            
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                OrderTime = DateTime.Now.TimeOfDay,
                OrderStatus = OrderStatus.Outstanding,
                User = user
            };
            context.Attach(order);
            
            foreach (MenuItem orderItem in orderItems)
            {
                OrdersDetail ordersDetail = new OrdersDetail
                {
                    Order = order,
                    MenuItem = orderItem,
                    ItemQuantity = orderItem.ItemBoughtCount
                };
                context.Attach(ordersDetail);
            }

            context.SaveChanges();
            
            HttpContext.Session.Remove("Order");
            return RedirectToAction("Confirmed");
        }

        [HttpPost]
        public IActionResult CancelPayment()
        {
            return RedirectToAction("View");
        }

        [HttpGet]
        public IActionResult Confirmed()
        {
            return View("Confirmed");
        }
        
        [HttpGet]
        [Route("Order/Add/{id:int}")]
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
               
                //Check to see if the item already exists
                MenuItem existingItem = order.FirstOrDefault(x => x.ItemId == item.ItemId);
                if (existingItem == null)
                {
                    order.Add(item);
                }
                else
                {
                    existingItem.ItemBoughtCount++;
                }

                HttpContext.Session.SetObjectAsJson("Order", order);
            }

            return RedirectToAction("View");
        }

        [HttpGet]
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
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
    /// <summary>
    ///     <see cref="Controller"/> for orders
    /// </summary>
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
        
        /// <summary>
        ///     Gets the main index page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            //Get all the menu items from the database
            MenuItem[] menuItems = context.MenuItems.ToArray();
            ViewBag.Order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            ViewBag.MenuItems = menuItems;
            return View("Index");
        }
        
        /// <summary>
        ///     Gets the view order page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public new IActionResult View()
        {
            List<MenuItem> order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
            ViewBag.Order = order;
            return View("View");
        }
        
        /// <summary>
        ///     Adds an <see cref="MenuItem"/> to the order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Order/Add/{id:int}")]
        public IActionResult Add(int id)
        {
            //Get the MenuItem from our database
            MenuItem item = context.MenuItems.FirstOrDefault(x => x.ItemId == id);
            
            //Invalid ID
            if (item == null)
                return RedirectToAction("View");
            
            //Get the Order from the session
            if (HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order") == null)
            {
                //The Order doesn't exist, so we will create it
                List<MenuItem> order = new List<MenuItem> {item};
                HttpContext.Session.SetObjectAsJson("Order", order);
            }
            else
            {
                //It exists, get the list
                List<MenuItem> order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
               
                //Check to see if the item already exists
                MenuItem existingItem = order.FirstOrDefault(x => x.ItemId == item.ItemId);
                
                //The item doesn't exist, add it
                if (existingItem == null)
                    order.Add(item);
                else //It does exist, increase how many times we have bought it
                    existingItem.ItemBoughtCount++;

                HttpContext.Session.SetObjectAsJson("Order", order);
            }

            return RedirectToAction("View");
        }

        /// <summary>
        ///     Removes an <see cref="Order"/> from the order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Order/Remove/{id:int}")]
        public IActionResult Remove(int id)
        {
	        if (HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order") != null)
	        {
		        List<MenuItem> order = HttpContext.Session.GetObjectFromJson<List<MenuItem>>("Order");
                
                MenuItem item = order.Find(x => x.ItemId == id);
		        if (item != null)
		        {
			        if (item.ItemBoughtCount == 1)
				        order.Remove(item);
			        else
				        item.ItemBoughtCount--;
		        }

                HttpContext.Session.SetObjectAsJson("Order", order);
	        }

	        return RedirectToAction("View");
        }

        #region Payment

        /// <summary>
        ///     Gets the payment view
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets the confirm payment view
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///     Redirects to the View view
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CancelPayment()
        {
            return RedirectToAction("View");
        }

        /// <summary>
        ///     Gets the Confirmed payment view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Confirmed()
        {
            return View("Confirmed");
        }
        
        #endregion

        /// <summary>
        ///     Gets the manage orders view
        /// </summary>
        /// <returns></returns>
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
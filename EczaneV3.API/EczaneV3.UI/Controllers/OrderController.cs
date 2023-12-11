using EczaneV3.Data.Abstract;
using EczaneV3.Data.Repositories;
using EczaneV3.Entites.Models.DataTable;
using EczaneV3.Entites.Models;
using EczaneV3.Entites.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EczaneV3.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace EczaneV3.UI.Controllers
{
	public class OrderController : Controller
	{
		private readonly IMedicamentRepository _medicamentRepository;
		private readonly IOrderItemRepository _orderItemRepository;
		private readonly IOrderRepository _orderRepository;
		private readonly EczaneV3DbContext _dbContext;


		public OrderController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IMedicamentRepository medicamentRepository, EczaneV3DbContext dbContext)
        {
            _orderRepository = orderRepository;
			_orderItemRepository = orderItemRepository;
			_medicamentRepository = medicamentRepository;
			_dbContext=dbContext;
	}

        public IActionResult Index()
		{
			return View();
		}

        public async Task<JsonResult> IndexDataAsync()
        {
            DataTable<Order> data = new DataTable<Order>();
            var ordersList = await _orderRepository.GetListAsync();
			var order = ordersList.Where(q=>q.UserName == User.Identity.Name).ToList();
			data.data = order;
            return Json(data);
        }

		public IActionResult Detail(string id)
		{
			ViewBag.Id = id;
			return View();
		}

		public JsonResult DetailDataAsync(string id)
		{
			Guid orderGuid = new Guid(id);
			DataTable<OrderItem> data = new DataTable<OrderItem>();
			var ordersList = _dbContext.Orders.Include(x => x.OrderItems).ToList();
			var order = ordersList.Where(q => q.Id == orderGuid).First();
			data.data = order.OrderItems;
			return Json(data);
		}

		[Authorize(Roles = "Admin,Muhasebe")]
		public IActionResult AllOrders()
		{
			return View();
		}

		[Authorize(Roles = "Admin,Muhasebe")]
		public async Task<JsonResult> AllOrdersDataAsync()
		{
			DataTable<Order> data = new DataTable<Order>();
			var ordersList = await _orderRepository.GetListAsync();
			data.data = ordersList;
			return Json(data);
		}

		[Authorize(Roles = "Admin,Muhasebe")]
		public IActionResult ConfirmOrders()
		{
			return View();
		}

		[Authorize(Roles = "Admin,Muhasebe")]
		public async Task<JsonResult> ConfirmOrdersDataAsync()
		{
			DataTable<Order> data = new DataTable<Order>();
			var ordersList = await _orderRepository.GetListAsync();
			ordersList=ordersList.Where(q=>q.Status==true).ToList();
			data.data = ordersList;
			return Json(data);
		}

		[Authorize(Roles = "Admin,Muhasebe,Kargo")]
		public IActionResult WaitingOrders()
		{
			return View();
		}

		[Authorize(Roles = "Admin,Muhasebe,Kargo")]
		public async Task<JsonResult> WaitingOrdersDataAsync()
		{
			DataTable<Order> data = new DataTable<Order>();
			var ordersList = await _orderRepository.GetListAsync();
			ordersList = ordersList.Where(q => q.Status == false).ToList();
			data.data = ordersList;
			return Json(data);
		}

		public async Task<IActionResult> Confirm(string id)
		{
			Guid orderGuid = new Guid(id);
			var orders = await _orderRepository.GetListAsync();
			var order = orders.Where(q=>q.Id==orderGuid).First();
			order.Status= true;
			await _orderRepository.UpdateAsync(order);
			
			return Redirect("/order/AllOrders");
		}

		public IActionResult Basket()
		{
			return View();
		}

		[HttpPost]
		public async Task<bool> BasketComplatedAsync(string json)
		{
            var siparisler = JsonConvert.DeserializeObject<List<BasketJson>>(json);
			if (siparisler != null) {
				Order order = new Order();
                var medicamentList = await _medicamentRepository.GetListAsync();

                foreach (var item in siparisler)
                {
                    Guid medicamentGuid = new Guid(item.id);

                    OrderItem orderItem = new OrderItem();

					orderItem.Medicament = medicamentList.Where(q => q.Id == medicamentGuid).First();
					orderItem.Amount = item.adet;
					orderItem.TotalPrice = item.totalPrice;
					if (await StokControlAsync(medicamentGuid, item.adet))
					{
						await _orderItemRepository.AddAsync(orderItem);
						order.OrderItems.Add(orderItem);
						await StokEksiltAsync(medicamentGuid, item.adet);

                    }
					
                }

				order.TotalPrice = siparisler.Sum(q=>q.totalPrice);
				order.UserName = User.Identity.Name;
				order.Status = false;
				await _orderRepository.AddAsync(order);
            }
            
            return true;
		}

		[NonAction]
		public async Task<bool> StokControlAsync(Guid id, int stok)
		{
            var medicamentList = await _medicamentRepository.GetListAsync();
			var medicament = medicamentList.Where(q=>q.Id == id).First();
			if (stok <= medicament.Stock)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        [NonAction]
		public async Task<bool> StokEksiltAsync(Guid id, int stok) 
		{

            var medicamentList = await _medicamentRepository.GetListAsync();
            var medicament = medicamentList.Where(q => q.Id == id).First();
			medicament.Stock -= stok;
			await _medicamentRepository.UpdateAsync(medicament);
			return true;
        }

    }
}

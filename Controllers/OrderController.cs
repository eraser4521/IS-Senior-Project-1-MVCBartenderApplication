using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCBarApplication.Data;
using MVCBarApplication.Models;

namespace MVCBarApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Menu()
        {
            var menu = await _context.Cocktails.ToListAsync();
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(int cocktailId, string customerName)
        {
            if (string.IsNullOrEmpty(customerName))
            {
                TempData["Error"] = "Please enter your name.";
                return RedirectToAction(nameof(Menu));
            }

            var cocktail = await _context.Cocktails.FindAsync(cocktailId);
            if (cocktail == null)
            {
                return NotFound();
            }

            var order = new Order
            {
                CustomerName = customerName,
                CocktailId = cocktailId,
                OrderTime = DateTime.Now,
                Status = OrderStatus.Ordered
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(OrderConfirmation), new { id = order.Id });
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders.Include(o => o.Cocktail).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        public async Task<IActionResult> Queue()
        {
            var orderQueue = await _context.Orders
                .Include(o => o.Cocktail)
                .Where(o => o.Status != OrderStatus.ReadyForPickup)
                .OrderBy(o => o.OrderTime)
                .ToListAsync();

            return View(orderQueue);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = OrderStatus.ReadyForPickup;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Queue));
        }
    }
}
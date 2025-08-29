using MVCBarApplication.Models;
using System.ComponentModel.DataAnnotations;

namespace MVCBarApplication.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        public int CocktailId { get; set; }
        public Cocktail? Cocktail { get; set; } 

        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class Restaurants
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RestaurantID { get; set; }
    public string RestaurantName { get; set; }
    public string CuisineType { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string OpeningHours { get; set; }
}
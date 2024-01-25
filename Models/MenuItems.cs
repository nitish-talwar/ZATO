using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class MenuItems
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemID { get; set; }
    [ForeignKey("Restaurant")]
    public int RestaurantID { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
}
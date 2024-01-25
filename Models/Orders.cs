using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class Orders
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderID { get; set; }
    [ForeignKey("User")]
    public int UserID { get; set; }
    [ForeignKey("Restaurant")]
    public int RestaurantID { get; set; }

    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public string DeliveryAddress { get; set; }
    public string Status { get; set; }
}
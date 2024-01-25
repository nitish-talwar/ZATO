using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class OrderItems
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderItemID { get; set; }
    [ForeignKey("Order")]
    public int OrderID { get; set; }
    [ForeignKey("MenuItems")]
    public int ItemID { get; set; }
    public int Quantity { get; set; }
    public double Subtotal { get; set; }
}
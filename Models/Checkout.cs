using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class Checkout
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CheckoutID { get; set; }
    [ForeignKey("User")]
    public int UserID { get; set; }
    public User User { get; set; }
    [ForeignKey("Orders")]
    public int OrderID { get; set; }

    public Orders Orders { get; set; }

    public DateTime CheckoutDate { get; set; }
    public double TotalAmount { get; set; }
}
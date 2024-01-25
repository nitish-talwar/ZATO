using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class Cart
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartID { get; set; }
    
    [ForeignKey("User")]
    public int UserID { get; set; }
    
    [ForeignKey("MenuItems")] 
    public int ItemID { get; set; }

    public int Quantity { get; set; }
}
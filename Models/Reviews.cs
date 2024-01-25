using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class Reviews
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReviewID { get; set; }
    [ForeignKey("User")]
    public int UserID { get; set; }
    [ForeignKey("Restaurant")]
    public int RestaurantID { get; set; }
    public int Rating { get; set; }

    public DateTime Date { get; set; }
}
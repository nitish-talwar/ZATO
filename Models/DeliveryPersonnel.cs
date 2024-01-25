using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class DeliveryPersonnel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeliveryPersonID { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string CurrentLocation { get; set; }
}
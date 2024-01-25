using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class DeliveryAgents
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeliveryAgentID { get; set; }
    public int UserID { get; set; }
    public int DeliveryPersonID { get; set; }
    public string VehicleType { get; set; }
    public string VehiclePlateNumber { get; set; }
}
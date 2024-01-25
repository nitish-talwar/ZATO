using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class DeliveryAssignments
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AssignmentID { get; set; }
    [ForeignKey("Order")]
    public int OrderID { get; set; }
    [ForeignKey("DeliveryPersonnel")]
    public int DeliveryAgentID { get; set; }
    public string AssignmentStatus { get; set; }
}
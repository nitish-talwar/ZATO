namespace ZATO.DTOs;

public class DeliveryAssignmentDTO
{
    public int OrderID { get; set; }
    public int DeliveryAgentID { get; set; }
    public string AssignmentStatus { get; set; }
}
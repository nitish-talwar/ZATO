namespace ZATO.DTOs;

public class OrderDTO
{
    public int UserID { get; set; }
    public int RestaurantID { get; set; }
    public DateTime OrderDate { get; set; }
    public double TotalAmount { get; set; }
    public string DeliveryAddress { get; set; }
    public string PaymentStatus { get; set; }
    public string Status { get; set; }
}
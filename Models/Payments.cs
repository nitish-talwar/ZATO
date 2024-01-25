namespace ZATO.Models;

public class Payments
{
    public int PaymentID { get; set; }
    public int UserID { get; set; }
    public int OrderID { get; set; }
    public double Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentStatus { get; set; }
}
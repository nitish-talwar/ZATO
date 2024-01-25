namespace ZATO.DTOs;

public class MenuItemsDTO
{
    public int ItemID { get; set; }
    public int RestaurantID { get; set; }
    public string ItemName { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Category { get; set; }
}
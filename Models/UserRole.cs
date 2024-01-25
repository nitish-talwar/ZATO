using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public class UserRole
{
    [ForeignKey("User")]
    public int UserId { get; set; }
    
    public int RoleId { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZATO.Models;

public partial class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }

    public bool PhoneNumberConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; }
}
using Microsoft.AspNetCore.Identity;

namespace AutoHub.Data.Models;

public class User : IdentityUser
{
    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    public virtual ICollection<Car> FavouriteCars { get; set; } = new List<Car>();
}
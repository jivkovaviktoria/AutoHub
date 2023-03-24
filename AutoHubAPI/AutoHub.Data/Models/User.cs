using Microsoft.AspNetCore.Identity;

namespace AutoHub.Data.Models;

public class User : IdentityUser
{
    public User()
    {
        this.Cars = new List<Car>();
    }
    public virtual ICollection<Car> Cars { get; set; }
}
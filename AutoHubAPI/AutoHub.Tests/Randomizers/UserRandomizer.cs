using AutoHub.Data.Models;
using TryAtSoftware.Randomizer.Core;
using TryAtSoftware.Randomizer.Core.Primitives;

namespace AutoHub.Tests.Randomizers;

public class UserRandomizer : ComplexRandomizer<User>
{
    public UserRandomizer()
    {
        this.AddRandomizationRule(x => x.Id, new StringRandomizer());
        this.AddRandomizationRule(x => x.UserName, new StringRandomizer());
        this.AddRandomizationRule(x => x.Email, new StringRandomizer());
    }
}
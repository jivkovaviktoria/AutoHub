using AutoHub.Core.Services;
using AutoHub.Data.Models;
using TryAtSoftware.Equalizer.Core;

namespace AutoHub.Tests.Services;

public class TokenServiceTests
{
    private readonly TokenService _tokenService;

    public TokenServiceTests()
    {
        this._tokenService = new TokenService();
    }
    
    [Fact]
    public void TestCreateTokenWithValidUserShouldBeSuccessfull()
    {
        var user = new User()
        {
            UserName = "Test",
            Email = "Test@gmail.com",
            Id = Guid.NewGuid().ToString()
        };

        var result = this._tokenService.CreateToken(user);
        
        Assert.IsType<string>(result);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void TestCreateTokenWithInvalidUserShouldNotBeSuccessfull()
    {
        var user = new User();
        Assert.Throws<ArgumentNullException>(() => this._tokenService.CreateToken(user));
    }
}
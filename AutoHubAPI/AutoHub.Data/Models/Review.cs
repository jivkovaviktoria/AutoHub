namespace AutoHub.Data.Models;

public class Review : BaseEntity
{
    public string Text { get; set; } = null!;

    public string OwnerId { get; set; } = null!;
    public User Owner { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}
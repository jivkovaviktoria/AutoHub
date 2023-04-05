using System.ComponentModel.DataAnnotations.Schema;

namespace AutoHub.Data.Models;

public class Image : BaseEntity
{
    public string Url { get; set; } = null!;

    public Guid CarId { get; set; }

    [ForeignKey(nameof(CarId))] 
    public Car Car { get; set; } = null!;
}
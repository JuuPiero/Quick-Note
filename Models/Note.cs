using System.ComponentModel.DataAnnotations;

namespace Note.Models;

public class Note
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }


    public virtual User? User { get; set; }
    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();

    
}

using System.ComponentModel.DataAnnotations;

namespace Note.Models;

public class Group
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public int UserId { get; set; }


    // Navigation Properties
    public virtual User Owner { get; set; }
    public virtual ICollection<Member> Members { get; set; } = new List<Member>();
    public virtual ICollection<Share> Shares { get; set; } = new List<Share>();
}
using System.ComponentModel.DataAnnotations;

namespace Note.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }


    // Navigation Properties
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
    public virtual ICollection<Group> Groups { get; set; } = new List<Group>();
    public virtual ICollection<Member> Memberships { get; set; } = new List<Member>();
}
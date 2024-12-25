using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Note.Models;

public class Member
{
    // [Key, Column(Order = 0)]
    public int UserId { get; set; }

    // [Key, Column(Order = 1)]
    public int GroupId { get; set; }
    
    public virtual User? User { get; set; }
    public virtual Group? Group { get; set; }
}
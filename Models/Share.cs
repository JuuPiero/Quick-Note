using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Note.Models;

public class Share
{
    [Key, Column(Order = 0)]
    public int NoteId { get; set; }

    [Key, Column(Order = 1)]
    public int GroupId { get; set; }

    // Navigation Properties
    public virtual Note? Note { get; set; }
    public virtual Group? Group { get; set; }

}
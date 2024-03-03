using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class News
{
    public int Id { get; set; }

    //public int? VideoId { get; set; }
    [Display(Name = "News Content")]
    public string NewsContent { get; set; } = null!;
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateTime DateModified { get; set; }
    [Display(Name = "Video")]
    //public virtual Video? Video { get; set; }
    public string? VideoURL { get; set; }
}

using CIADatabase.Areas.GWOT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CIADatabase.Areas.GWOT
{
    public class GWOTSections
    {
        [Key]
        public int SectionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<GWOTArticle> Articles { get; set; }
    }
}
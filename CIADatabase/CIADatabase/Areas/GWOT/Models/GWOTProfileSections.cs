using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CIADatabase.Areas.GWOT.Models
{
    public class GWOTProfileSections
    {
        [Key]
        public int SectionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<GWOTProfile> Profiles { get; set; }
    }
}
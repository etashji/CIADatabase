using CIADatabase.Areas.JediArchives.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CIADatabase.Areas.JediArchives
{
    public class JediSection
    {
        [Key]
        public int SectionId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public virtual ICollection<JediArticle> Articles { get; set; }
    }
}

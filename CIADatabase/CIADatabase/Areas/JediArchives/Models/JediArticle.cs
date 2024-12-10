using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace CIADatabase.Areas.JediArchives.Models
{
    public class JediArticle
    {
        [Key]
        public int JediArticleId { get; set; }

        // DateTimes for local and Zulu time
        [Required]
        public int Order { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        // Location of the event
        [StringLength(200)]
        public string Planet { get; set; }

        // Location of the event
        [StringLength(200)]
        public string Location { get; set; }

        // Map Images (stored as byte arrays)
        public byte[] Starmap1 { get; set; }
        public byte[] Starmap2 { get; set; }
        public byte[] Starmap3 { get; set; }
        public byte[] LocationMap1 { get; set; }
        public byte[] LocationMap2 { get; set; }
        public byte[] LocationMap3 { get; set; }

        // Text sections
        [AllowHtml]
        [Required]
        [Column(TypeName = "ntext")]
        [MinLength(10, ErrorMessage = "The briefing must be at least 10 characters long.")]
        public string Briefing { get; set; }

        [MinLength(10, ErrorMessage = "The video link must be at least 10 characters long.")]
        public string Video { get; set; }

        [AllowHtml]
        [Column(TypeName = "ntext")]
        [MinLength(10, ErrorMessage = "The After Action Report must be at least 10 characters long.")]
        [Display(Name = "After Action Report")]
        public string AfterActionReport { get; set; }

        // Foreign key for Section
        public int JediSectionId { get; set; }
        public virtual JediSection JediSection { get; set; }
    }
}

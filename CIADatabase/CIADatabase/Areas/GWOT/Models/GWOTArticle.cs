using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace CIADatabase.Areas.GWOT.Models
{
    public class GWOTArticle
    {
        [Key]
        public int GWOTArticleId { get; set; }

        // DateTimes for local and Zulu time
        [Required]
        public DateTime LocalTime { get; set; } = new DateTime(1753, 1, 1);

        [Required]
        [Display(Name = "Zulu Time")]
        public DateTime ZuluTime { get; set; } = new DateTime(1753, 1, 1);

        // Location of the event
        [StringLength(200)]
        public string Location { get; set; }

        // Map Images (stored as byte arrays)
        public byte[] PoliticalMap { get; set; }
        public byte[] MilitaryMap { get; set; }
        public byte[] PresidentRegionMap { get; set; }
        public byte[] CountryMap { get; set; }
        public byte[] RegionMap { get; set; }
        public byte[] LocalMap { get; set; }
        public byte[] CityMap { get; set; }

        // Text sections
        [AllowHtml]
        [Required]
        [Column(TypeName = "ntext")]
        [MinLength(10, ErrorMessage = "The briefing must be at least 10 characters long.")]// Adjust column type if using SQL Server
        public string Briefing { get; set; }

        [MinLength(10, ErrorMessage = "The video link must be at least 10 characters long.")]
        public string Video { get; set; }

        public byte[] UpdatedPoliticalMap { get; set; }
        public byte[] UpdatedMilitaryMap { get; set; }
        public byte[] UpdatedRegionMap { get; set; }

        [AllowHtml]
        [Required]
        [Column(TypeName = "ntext")]
        [MinLength(10, ErrorMessage = "The video link must be at least 10 characters long.")]
        [Display(Name = "After Action Report")]
        public string AfterActionReport { get; set; }

        // Foreign key for Section
        public int GWOTSectionId { get; set; }
        public virtual GWOTSections GWOTSection { get; set; }
    }
}
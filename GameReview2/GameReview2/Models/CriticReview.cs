using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameReview2.Models
{
    public class CriticReview
    {
        [Key]
        public int CriticReviewId { get; set; }

        [StringLength(255)]
        [Display(Name = "Critic's Name")]
        public string CriticFullName { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CriticCreatedOn { get; set; }

        [Display(Name = "Latest Update")]
        public DateTime CriticUpdatedOn { get; set; }

        [Range(0, 100)]
        [Display(Name = "Score")]
        public int CriticScore { get; set; }

        [Display(Name = "Review")]
        public string CriticRev { get; set; }

        [Display(Name = "Game")]
        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
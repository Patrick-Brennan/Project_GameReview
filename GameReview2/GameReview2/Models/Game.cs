using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameReview2.Models
{
    public class Game
    {
        [Key]
        public int GameId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Display(Name = "Game Info")]
        public int GameInfoId { get; set; }

        public GameInfo GameInfo { get; set; }

        [Required]
        public string Summary { get; set; }

        [Display(Name = "Critic's Average Score")]
        public int CriticScoreAvg { get; set; }

        [Display(Name = "User's Average Score")]
        public int UserScoreAvg { get; set; }

        [Display(Name = "Critic Reviews")]
        public int CriticReviewCount { get; set; }

        [Display(Name = "User Reviews")]
        public int UserReviewCount { get; set; }

        public byte[] Photo { get; set; }
    }
}
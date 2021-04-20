using GameReview2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameReview2.ViewModels
{
    public class GameViewModel
    {
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

        public int CriticReviewId { get; set; }

        public CriticReview CriticReview { get; set; }

        [Display(Name = "Critic Reviews")]
        public int CriticReviewCount { get; set; }

        public int UserReviewId { get; set; }

        public UserReview UserReview { get; set; }

        [Display(Name = "User Reviews")]
        public int UserReviewCount { get; set; }

        public HttpPostedFileBase Photo { get; set; }
        public byte[] PhotoDB { get; set; }

        public virtual ICollection<CriticReview> CriticReviews { get; set; }

        public virtual ICollection<UserReview> UserReviews { get; set; }
    }
}
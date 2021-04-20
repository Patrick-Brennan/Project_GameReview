using GameReview2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameReview2.ViewModels
{
    public class UserReviewViewModel
    {
        public int UserReviewId { get; set; }

        [StringLength(255)]
        [Display(Name = "User's Name")]
        public string UserFullName { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime UserCreatedOn { get; set; }

        [Display(Name = "Latest Update")]
        public DateTime UserUpdatedOn { get; set; }

        [Range(0, 100)]
        [Display(Name = "Score")]
        public int UserScore { get; set; }

        [Display(Name = "Review")]
        public string UserRev { get; set; }

        [Display(Name = "Game Info")]
        public int GameId { get; set; }

        public Game Game { get; set; }
    }
}
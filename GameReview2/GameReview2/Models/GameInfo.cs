using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameReview2.Models
{
    public class GameInfo
    {
        [Key]
        public int GameInfoId { get; set; }

        [Display(Name = "Genre(s): ")]
        public GenreSelect Genre { get; set; }

        [Display(Name = "Rating: ")]
        public RatingSelect Rating { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace GameReview2.Models
{
    public enum RatingSelect
    {
        E,
        [Display(Name = "E+10")]
        ETen,
        T,
        M
    }
}
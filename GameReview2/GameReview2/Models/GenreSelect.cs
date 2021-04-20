using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace GameReview2.Models
{
    public enum GenreSelect
    {
        Action,
        Adventure,
        Casual,
        Coop,
        Exploration,
        Fighting,
        Horror,
        JRPG,
        Multiplayer,
        Platform,
        Rhythm,
        RPG,
        Sandbox,
        Shooter,
        Stealth,
        Strategy,
        Survival,
    }

    public class Tests
    {
        public GenreSelect[] genreSelect { get; set; }
    }
}
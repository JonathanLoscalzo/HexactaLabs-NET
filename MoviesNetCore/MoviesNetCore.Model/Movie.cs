﻿using System;
using System.Collections.Generic;

namespace MoviesNetCore.Model
{
    public class Movie
    {
        public Movie()
        {
            this.Genres = new HashSet<Genre>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string Plot { get; set; }

        public string CoverLink { get; set; }

        public int? Runtime { get; set; }

        public virtual ICollection<Genre> Genres { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FirstDotNetWebApp.Models
{
    public class MovieGenreViewModel
    {
        public List<Movie> movies;
        public SelectList genres;
        public string movieGenre { get; set; }
    }
}
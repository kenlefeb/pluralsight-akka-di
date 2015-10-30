using System;

namespace MovieStreaming.Exceptions
{
    public class SimulatedTerribleMovieException : Exception
    {
        public string MovieTitle { get; private set; }

        public SimulatedTerribleMovieException(string movieTitle) 
            : base(string.Format("{0} is a terrible movie",movieTitle))
        {
            MovieTitle = movieTitle;
        }
    }
}
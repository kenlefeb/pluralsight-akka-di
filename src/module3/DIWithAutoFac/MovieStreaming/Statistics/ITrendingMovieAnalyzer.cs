using System.Collections.Generic;

namespace MovieStreaming.Statistics
{
    public interface ITrendingMovieAnalyzer
    {
        string CalculateMostPopularMovie(IEnumerable<string> movieTitles);
    }
}

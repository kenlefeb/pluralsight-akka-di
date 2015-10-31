using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using MovieStreaming.Messages;
using MovieStreaming.Statistics;

namespace MovieStreaming.Actors
{
    public class TrendingMoviesActor : ActorBase
    {
        private readonly ITrendingMovieAnalyzer _trendAnalyzer;

        private readonly Queue<string> _recentlyPlayedMovies;
        private const int NumberOfRecentMoviesToAnalyze = 5;

        public TrendingMoviesActor(ITrendingMovieAnalyzer analyzer)
        {
            _recentlyPlayedMovies = new Queue<string>(NumberOfRecentMoviesToAnalyze);
            _trendAnalyzer = analyzer;

            Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            var recentlyPlayedMoviesBufferIsFull = _recentlyPlayedMovies.Count == NumberOfRecentMoviesToAnalyze;

            if (recentlyPlayedMoviesBufferIsFull)
            {
                // remove oldest movie title
                _recentlyPlayedMovies.Dequeue();
            }

            _recentlyPlayedMovies.Enqueue(message.MovieTitle);

            LogDebug();


            var topMovie = _trendAnalyzer.CalculateMostPopularMovie(_recentlyPlayedMovies);

            _logger.Info("TrendingMovieActor Most popular movie trending now is {MovieTitle}", topMovie);
        }

        private void LogDebug()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Recently played movies:");

            foreach (var movie in _recentlyPlayedMovies)
            {
                sb.AppendLine(movie);
            }

            _logger.Debug("TrendingMovieActor {RecentlyPlayedMovies}", sb);
        }
    }
}

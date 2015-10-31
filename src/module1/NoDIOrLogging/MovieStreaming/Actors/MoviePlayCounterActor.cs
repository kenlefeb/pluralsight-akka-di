using System;
using System.Collections.Generic;
using Akka.Actor;
using MovieStreaming.Exceptions;
using MovieStreaming.Messages;


namespace MovieStreaming.Actors
{
    public class MoviePlayCounterActor : ActorBase
    {
        private readonly Dictionary<string, int> _moviePlayCounts;

        public MoviePlayCounterActor()
        {
            _moviePlayCounts = new Dictionary<string, int>();

            Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            if (_moviePlayCounts.ContainsKey(message.MovieTitle))
            {
                _moviePlayCounts[message.MovieTitle]++;
            }
            else
            {
                _moviePlayCounts.Add(message.MovieTitle, 1);
            }

            //  Simulated bugs
            if (message.MovieTitle == "Partial Recoil")
            {
                throw new SimulatedTerribleMovieException(message.MovieTitle);
            }

            if (message.MovieTitle == "Partial Recoil 2")
            {
                throw new InvalidOperationException("Simulated exception");
            }

            _logger.Info("MoviePlayCounterActor {MovieTitle} has been watched {MoviePlayCount} times", message.MovieTitle, _moviePlayCounts[message.MovieTitle]);
        }

    }
}

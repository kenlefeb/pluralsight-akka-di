using System;
using Akka.Actor;
using Akka.Event;
using MovieStreaming.Messages;

namespace MovieStreaming.Actors
{
    public class UserActor : ActorBase
    {
        private readonly int _userId;
        private string _currentlyWatching;

        public UserActor(int userId)
        {
            _userId = userId;
            _loggingIdentifier = string.Format("{0} {1}", GetType().Name, _userId);

            Stopped();
        }

        private void Playing()
        {
            Receive<PlayMovieMessage>(
                message =>
                {
                   _logger.Warning("UserActor {UserId} cannot start playing another movie before stopping existing one", _userId);
                });

            Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());

            _logger.Info("UserActor {UserId} behaviour has now become Playing", _userId);
        }

        private void Stopped()
        {
            Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));

            Receive<StopMovieMessage>(
                message =>
                {
                    _logger.Warning("UserActor {UserId} cannot stop if nothing is playing", _userId);
                }
                );

            _logger.Info("UserActor {UserId} behaviour has now become Stopped", _userId);
        }

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;

            _logger.Info("UserActor {UserId} is currently watching _currentlyWatching", _userId);

            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                .Tell(new IncrementPlayCountMessage(title));

            Context.ActorSelection("/user/Playback/PlaybackStatistics/TrendingMovies")
                .Tell(new IncrementPlayCountMessage(title));

            Become(Playing);
        }

        private void StopPlayingCurrentMovie()
        {
            _logger.Info("UserActor {UserId} has stopped watching _currentlyWatching", _userId);

            _currentlyWatching = null;

            Become(Stopped);
        }

    }
}

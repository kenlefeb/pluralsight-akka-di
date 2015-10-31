using System;
using Akka.Actor;
using Akka.Event;
using MovieStreaming.Exceptions;

namespace MovieStreaming.Actors
{
    public class PlaybackStatisticsActor : ActorBase
    {
        private ILoggingAdapter _logger = Context.GetLogger();

        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
            Context.ActorOf(Props.Create<TrendingMoviesActor>(), "TrendingMovies");
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {

            return new OneForOneStrategy(
                exception =>
                {
                    if (exception is ActorInitializationException)
                    {
                        _logger.Error(exception, "PlaybackStatisticsActor PlaybackStatisticsActor supervisor strategy stopping child due to ActorInitializationException");

                        return Directive.Stop;
                    }

                    if (exception is SimulatedTerribleMovieException)
                    {
                        var terribleMovieEx = (SimulatedTerribleMovieException) exception;

                        _logger.Error(exception, "PlaybackStatisticsActor supervisor strategy resuming child due to terrible movie {MovieTitle}", terribleMovieEx.MovieTitle);

                        return Directive.Resume;
                    }

                    _logger.Error("PlaybackStatisticsActor supervisor strategy restarting child due to unexpected exception");
                    return Directive.Restart;
                }
                );

        }

   }
}
﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Akka.Actor;
using Akka.Event;
using MovieStreaming.Exceptions;
using MovieStreaming.Messages;


namespace MovieStreaming.Actors
{
    public class MoviePlayCounterActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();

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
                var currentCount = _moviePlayCounts[message.MovieTitle];

                currentCount++;

                _moviePlayCounts[message.MovieTitle] = currentCount;
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

            _logger.Info("MoviePlayCounterActor {Movie} has been watched {Times} times", message.MovieTitle, _moviePlayCounts[message.MovieTitle]);
            
        }



        #region Lifecycle hooks

        protected override void PreStart()
        {
            _logger.Debug("MoviePlayCounterActor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("MoviePlayCounterActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("MoviePlayCounterActor PreRestart because {Reason}", reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("MoviePlayCounterActor PostRestart because {Reason}", reason);

            base.PostRestart(reason);
        }
        #endregion
    }
}

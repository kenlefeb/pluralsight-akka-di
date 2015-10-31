using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

namespace MovieStreaming.Actors
{
    public abstract class ActorBase : ReceiveActor
    {
        protected ILoggingAdapter _logger = Context.GetLogger();
        protected string _loggingIdentifier;

        public ActorBase()
        {
            _loggingIdentifier = GetType().Name;
        }

        protected override void PreStart()
        {
            _logger.Debug("{Actor} PreStart", _loggingIdentifier);
        }

        protected override void PostStop()
        {
            _logger.Debug("{Actor} PostStop", _loggingIdentifier);
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("{Actor} PreRestart because {1}", _loggingIdentifier, reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("{Actor} PostRestart because {1}", _loggingIdentifier, reason);

            base.PostRestart(reason);
        }
    }
}

using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using MovieStreaming.Messages;

namespace MovieStreaming.Actors
{
    public class UserCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> _users;
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public UserCoordinatorActor()
        {
            _users = new Dictionary<int, IActorRef>();

            Receive<PlayMovieMessage>(
                message =>
                {
                    CreateChildUserIfNotExists(message.UserId);

                    IActorRef childActorRef = _users[message.UserId];

                    childActorRef.Tell(message);
                });

            Receive<StopMovieMessage>(
                message =>
                {
                    CreateChildUserIfNotExists(message.UserId);

                    IActorRef childActorRef = _users[message.UserId];

                    childActorRef.Tell(message);
                });
        }


        private void CreateChildUserIfNotExists(int userId)
        {
            if (!_users.ContainsKey(userId))
            {
                IActorRef newChildActorRef = 
                    Context.ActorOf(Props.Create(() => new UserActor(userId)), "User" + userId);

                _users.Add(userId, newChildActorRef);

                _logger.Debug("UserCoordinatorActor created new child UserActor for {0}", userId);

                _logger.Info("Total Users {0}", _users.Count);
            }
        }


        #region Lifecycle hooks
        protected override void PreStart()
        {
            _logger.Debug("UserCoordinatorActor PreStart");
        }

        protected override void PostStop()
        {
            _logger.Debug("UserCoordinatorActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            _logger.Debug("UserCoordinatorActor PreRestart because {0}", reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("UserCoordinatorActor PostRestart because {0}", reason);

            base.PostRestart(reason);
        } 
        #endregion
    }
}
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

                _logger.Debug("UserCoordinatorActor created new child UserActor for {User}", userId);

                _logger.Info("Total Users {TotalUsers}", _users.Count);
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
            _logger.Debug("UserCoordinatorActor PreRestart because {Reason}", reason);

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            _logger.Debug("UserCoordinatorActor PostRestart because {Reason}", reason);

            base.PostRestart(reason);
        } 
        #endregion
    }
}
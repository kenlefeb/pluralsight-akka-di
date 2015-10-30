using System;
using System.Collections.Generic;
using Akka.Actor;
using MovieStreaming.Messages;

namespace MovieStreaming.Actors
{
    public class UserCoordinatorActor : ActorBase
    {
        private readonly Dictionary<int, IActorRef> _users;

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

                _logger.Info("UserCoordinatorActor created new child UserActor for {0}", userId);
                _logger.Info("Total Users {0}", _users.Count);
            }
        }
    }
}
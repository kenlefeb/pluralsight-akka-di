using System;
using Akka.Actor;


namespace MovieStreaming.Actors
{
    public class PlaybackActor : ActorBase
    {
        public PlaybackActor()
        {
            Context.ActorOf(Props.Create<UserCoordinatorActor>(), "UserCoordinator");
            Context.ActorOf(Props.Create<PlaybackStatisticsActor>(), "PlaybackStatistics");
        }
    }
}

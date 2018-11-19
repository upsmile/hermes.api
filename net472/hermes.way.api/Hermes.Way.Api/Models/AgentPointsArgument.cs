using System;

namespace Hermes.Way.Api.Services
{

    public class AgentPointsArgument
    {
        public object Result { get; set; }

        public Exception Exception { get; set; }
    }

    public delegate void AgentPointsEvent(AgentPointsArgument argument);

}

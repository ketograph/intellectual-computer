using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ic_project_2
{
    public class States
    {
        public State[] CurrentStates { get; set; } = new State[] { State.Undefined(), State.Undefined(), State.Undefined(), State.Undefined(), State.Undefined() };

        public State GetResultingState()
        {
            if (CurrentStates.Any(x => x.Status == State.InternalStatus.Alarm))
                return State.Alarm();
            else if (CurrentStates.Any(x => x.Status == State.InternalStatus.Warning))
                return State.Warning();
            else if (CurrentStates.Any(x => x.Status == State.InternalStatus.Good))
                return State.Good();
            else
                return State.Undefined();
        }
    }
}

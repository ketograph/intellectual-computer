using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ic_project_2
{
    public class State : IComparable
    {
        public enum InternalStatus
        {
            Good,
            Warning,
            Alarm,
            Undefined
        };

        public InternalStatus Status { get; set; }

        public int GetStateCode()
        {
            switch (this.Status)
            {
                case InternalStatus.Good:
                    return 1;
                case InternalStatus.Warning:
                    return 2;
                case InternalStatus.Alarm:
                    return 3;
                case InternalStatus.Undefined:
                    return -1;
                default:
                    return -1;
            }
        }

        public Color GetStateColor()
        {
            switch (this.Status)
            {
                case InternalStatus.Good:
                    return Color.LightGreen;
                case InternalStatus.Warning:
                    return Color.Yellow;
                case InternalStatus.Alarm:
                    return Color.Red;
                case InternalStatus.Undefined:
                    return Color.Gray;
                default:
                    return Color.Gray;
            }
        }
        public static State Good()
        {
            return new State() { Status = State.InternalStatus.Good };
        }
        public static State Undefined()
        {
            return new State() { Status = State.InternalStatus.Undefined };
        }
        public static State Warning()
        {
            return new State() { Status = State.InternalStatus.Warning };
        }
        public static State Alarm()
        {
            return new State() { Status = State.InternalStatus.Alarm };
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            State otherState = obj as State;
            if (otherState != null)
                return this.GetStateCode().CompareTo(otherState.GetStateCode());
            else
                throw new ArgumentException("Object is not a Temperature");
        }
    }


}

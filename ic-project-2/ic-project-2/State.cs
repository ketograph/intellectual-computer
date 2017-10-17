using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ic_project_2
{
    public class State
    {
        public enum InternalStatus
        {
            Good,
            Warning,
            Alarm
        };

        public InternalStatus Status { get; set; }

        public Color GetStateColor()
        {
            switch (this.Status)
            {
                case InternalStatus.Good:
                    return Color.Green;
                case InternalStatus.Warning:
                    return Color.Yellow;
                case InternalStatus.Alarm:
                    return Color.Red;
                default:
                    return Color.Gray;
            }
        }
    }
}

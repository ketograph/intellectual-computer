using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ic_project_2
{
    public class SensorValues
    {
        public int[] Values { get; set; } = new int[] { 0, 0, 0, 0, 0 };

        

        public void ParseSensorValuesString(string sensorsString)
        {
            if (string.IsNullOrEmpty(sensorsString))
                return;
            if (!IsSensorStringCorrect(sensorsString))
                return;
            var sensorValues = sensorsString.Split(';');

            Values[0] = int.Parse(sensorValues[1]);
            Values[1] = int.Parse(sensorValues[2]);
            Values[2] = int.Parse(sensorValues[3]);
            Values[3] = int.Parse(sensorValues[4]);
            Values[4] = int.Parse(sensorValues[5]);
        }

        private bool IsSensorStringCorrect(string sensorString)
        {
            if (string.IsNullOrEmpty(sensorString))
                return false;
            if (!sensorString.Contains(";"))
                return false;
            if (!(sensorString.StartsWith("VALUES") || sensorString.StartsWith("SET")))
                return false;
            var numbers = sensorString.Split(';');

            // check if there are at least 4 values
            if (numbers.Length <= 4)
                return false;

            // check if each of the first five values is not empty or longer then 4 chars
            for (int n = 1; n <= 5; n++)
            {
                if (numbers[n].Length == 0 || numbers[n].Length > 4)
                    return false;
            }
            return true;
        }

    }
}

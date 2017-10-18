using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ic_project_2
{
    public class SensorSetValues
    {
        public int[] SensorValue { get; set; } = new int[] { 0, 0, 0, 0, 0 };
        public bool[] SetValues { get; set; } = new bool[] { false, false, false, false, false };


        public void ParseSensorValuesString(string sensorsString)
        {
            if (string.IsNullOrEmpty(sensorsString))
                return;
            if (!IsSensorStringCorrect(sensorsString))
                return;
            var sensorValues = sensorsString.Split(';');

            SensorValue[0] = int.Parse(sensorValues[0]);
            SensorValue[1] = int.Parse(sensorValues[1]);
            SensorValue[2] = int.Parse(sensorValues[2]);
            SensorValue[3] = int.Parse(sensorValues[3]);
            SensorValue[4] = int.Parse(sensorValues[4]);

            SetValues[0] = Convert.ToBoolean(int.Parse(sensorValues[5]));
            SetValues[1] = Convert.ToBoolean(int.Parse(sensorValues[6]));
            SetValues[2] = Convert.ToBoolean(int.Parse(sensorValues[7]));
            SetValues[3] = Convert.ToBoolean(int.Parse(sensorValues[8]));
            SetValues[4] = Convert.ToBoolean(int.Parse(sensorValues[9]));
        }

        // Example:
        // 384;408;398;401;395;0;0;0;0;0;
        private bool IsSensorStringCorrect(string sensorString)
        {
            if (string.IsNullOrEmpty(sensorString))
                return false;
            if (!sensorString.Contains(";"))
                return false;
            // remove trailing ; and \n before splitting
            var numbers = sensorString.TrimEnd('\n').TrimEnd(';').Split(';');

            // check if there are 10 values
            if (numbers.Count() != 10)
                return false;
            // check if each of the first five values is not empty or longer then 4 chars
            int n = 0;
            for (n = 0; n <= 4; n++)
            {
                if (numbers[n].Length == 0 || numbers[n].Length > 4)
                    return false;
            }
            // check if the other values are only 1 or 0
            for (n = 5; n <= 9; n++)
            {
                if (!(numbers[n].Equals("1") || numbers[n].Equals("0")))
                    return false;
            }
            return true;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ic_project_2
{
    public class MeasuredParameters
    {
        public int[] Parameters { get; set; } = new int[] { 0, 0, 0, 0, 0 };

        

        public void ParseParametersString(string parametersString)
        {
            if (string.IsNullOrEmpty(parametersString))
                return;
            if (!IsParameterStringCorrect(parametersString))
                return;
            var parameters = parametersString.Split(';');

            Parameters[0] = int.Parse(parameters[0]);
            Parameters[1] = int.Parse(parameters[1]);
            Parameters[2] = int.Parse(parameters[2]);
            Parameters[3] = int.Parse(parameters[3]);
            Parameters[4] = int.Parse(parameters[4]);
        }

        private bool IsParameterStringCorrect(string parameterString)
        {
            if (string.IsNullOrEmpty(parameterString))
                return false;
            if (!parameterString.Contains(";"))
                return false;
            var numbers = parameterString.Split(';');

            // check if there are at least 4 values
            if (numbers.Length <= 4)
                return false;

            // check if each of the first five values is not empty or longer then 4 chars
            for (int n = 0; n <= 4; n++)
            {
                if (numbers[n].Length == 0 || numbers[n].Length > 4)
                    return false;
            }
            return true;
        }

    }
}

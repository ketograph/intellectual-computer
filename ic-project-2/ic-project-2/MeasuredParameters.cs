using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ic_project_2
{
    public class MeasuredParameters
    {
        public int Parameter1 { get; set; }
        public int Parameter2 { get; set; }
        public int Parameter3 { get; set; }
        public int Parameter4 { get; set; }
        public int Parameter5 { get; set; }

        public void ParseParametersString(string parametersString)
        {
            if (string.IsNullOrEmpty(parametersString))
                return;
            if (!IsParameterStringCorrect(parametersString))
                return;
            var parameters = parametersString.Split(';');
            Parameter1 = int.Parse(parameters[0]);
            Parameter2 = int.Parse(parameters[1]);
            Parameter3 = int.Parse(parameters[2]);
            Parameter4 = int.Parse(parameters[3]);
            Parameter5 = int.Parse(parameters[4]);
        }

        public List<int> ParameterList
        {
            get
            {
                return new List<int>
                {
                    Parameter1,
                    Parameter2,
                    Parameter3,
                    Parameter4,
                    Parameter5
                };
            }
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

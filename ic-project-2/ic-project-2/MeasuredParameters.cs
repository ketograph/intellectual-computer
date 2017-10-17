using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ic_project_2
{
    public class MeasuredParameters : INotifyPropertyChanged
    {
        // These fields hold the values for the public properties.
        private int _parameter1 = 0;
        private int _parameter2 = 0;
        private int _parameter3 = 0;
        private int _parameter4 = 0;
        private int _parameter5 = 0;

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public int Parameter1
        {
            get { return _parameter1; }
            set
            {
                if (value != _parameter1)
                {
                    _parameter1 = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int Parameter2
        {
            get { return _parameter2; }
            set
            {
                if (value != this._parameter2)
                {
                    this._parameter2 = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public int Parameter3
        {
            get { return _parameter3; }
            set
            {
                if (value != this._parameter3)
                {
                    this._parameter3 = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int Parameter4
        {
            get { return _parameter4; }
            set
            {
                if (value != this._parameter4)
                {
                    this._parameter4 = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public int Parameter5
        {
            get { return _parameter5; }
            set
            {
                if (value != this._parameter5)
                {
                    this._parameter5 = value;
                    NotifyPropertyChanged();
                }
            }
        }
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

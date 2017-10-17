using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace ic_project_2
{
    public partial class Form1 : Form
    {
        SerialPort serialPort;
        KnowledgeBase kb;
        List<TextBox> textBoxList; // to iterate over all textboxes
        MeasuredParameters parameters = new MeasuredParameters();

        public Form1()
        {
            InitializeComponent();
        }

        ~Form1()
        {
            serialPort?.Close();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string inParameterString = sp.ReadExisting();
            parameters.ParseParametersString(inParameterString);
        //    SetParametersTextboxes(parameters);
            QueryEachParameterSeperatly(parameters);
        }

        private void QueryEachParameterSeperatly(MeasuredParameters parameters)
        {
            var paramList = parameters.ParameterList;
            for (int param = 0; param <= 4; param++)
            {
                var state = kb.AskOneParameter(param, paramList[param]);
                SetStatus(param, state);
            }
        }

        private void SetStatus(int param, State state)
        {
            if (textBoxList[param].InvokeRequired)
            {
                Action act = () => textBoxList[param].BackColor = state.GetStateColor();
                textBoxList[param].Invoke(act);
            }
            else
            {
                textBoxList[param].BackColor = state.GetStateColor();
            }
        }

        public void SetParametersTextboxes(MeasuredParameters parameters)
        {

            for (int n = 0; n < 5; n++)
            {
                if (textBoxList[n].InvokeRequired)
                {
                    Action act = () => textBoxList[n].Text = parameters.ParameterList[n].ToString();
                    textBoxList[n].Invoke(act);
                }
                else
                {
                    textBoxList[n].Text = parameters.ParameterList[n].ToString();
                }
            }
        }

        private void comboBoxComPort_DropDown(object sender, EventArgs e)
        {
            comboBoxComPort.Items.AddRange(GetAllComPorts().ToArray());
        }

        private void comboBoxComPort_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string comPortString = comboBoxComPort.SelectedItem.ToString();
            OpenComPort(comPortString);
        }

        private void OpenComPort(string comPortString)
        {
            serialPort = new SerialPort(comPortString);
            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.BaudRate = 9600;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serialPort.Open();
                }
                catch (Exception ex)
                {
                    StatusLabelBar.Text = ex.Message;
                }
            }
            if (serialPort.IsOpen)
            {
                StatusLabelBar.Text = "Port " + comPortString + " is open.";
            }

        }
        private void OnFormLoad(object sender, EventArgs e)
        {
            textBoxList = new List<TextBox>() { textBoxPar1, textBoxPar2, textBoxPar3, textBoxPar4, textBoxPar5 };
            kb = new KnowledgeBase();
            kb.LogMessageAdded += this.OnNewLogMessage;
            kb.LoadTurtleFile();
            textBoxPar1.DataBindings.Add("Text", parameters, "Parameter1");
            textBoxPar2.DataBindings.Add("Text", parameters, "Parameter2");
            //textBoxPar3.DataBindings.Add("Text", parameters.Parameter3 "Name");
            //textBoxPar4.DataBindings.Add("Text", parameters.Parameter4, "Name");
            //textBoxPar5.DataBindings.Add("Text", parameters.Parameter5, "Name");

        }

        private void OnNewLogMessage(object source, LogMessageEventArgs e)
        {
            if (listBox1.InvokeRequired)
            {
                Action act = () =>
                {
                    listBox1.Items.Add(e.Message);
                    listBox1.TopIndex = listBox1.Items.Count - 1; // scroll to bottom
                };
                listBox1.Invoke(act);
            }
            else
            {
                listBox1.Items.Add(e.Message);
                listBox1.TopIndex = listBox1.Items.Count - 1; // scroll to bottom

            }
        }


        public List<string> GetAllComPorts()
        {
            List<String> allPorts = new List<String>();
            foreach (String portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                allPorts.Add(portName);
            }
            return allPorts;
        }
    }
}

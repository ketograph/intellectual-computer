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

namespace IntelligentComuting1
{
    public partial class Form1 : Form
    {
        SerialPort serialPort;
        delegate string ReadTextCallback();
        public Form1()
        {
            InitializeComponent();
            serialPort = new SerialPort("Com5");
            serialPort.BaudRate = 9600;
            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            serialPort.Open();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            SetTextBox(indata);
        }

        public void SetTextBox(string value)
        {
            if (value.Length < 10)
                return;
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(SetTextBox), new object[] { value });
            }
            receivedText.Text = value;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(sendText.Text != "")
            {
                serialPort.WriteLine(sendText.Text);
            }
        }
    }
}

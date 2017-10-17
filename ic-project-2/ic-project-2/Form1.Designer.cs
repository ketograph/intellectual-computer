
namespace ic_project_2
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxPar1 = new System.Windows.Forms.TextBox();
            this.textBoxPar2 = new System.Windows.Forms.TextBox();
            this.textBoxPar3 = new System.Windows.Forms.TextBox();
            this.textBoxPar4 = new System.Windows.Forms.TextBox();
            this.textBoxPar5 = new System.Windows.Forms.TextBox();
            this.labelPar2 = new System.Windows.Forms.Label();
            this.labelPar1 = new System.Windows.Forms.Label();
            this.labelPar3 = new System.Windows.Forms.Label();
            this.labelPar5 = new System.Windows.Forms.Label();
            this.labelPar4 = new System.Windows.Forms.Label();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabelBar = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.measuredParametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.form1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measuredParametersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxPar1
            // 
            this.textBoxPar1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.form1BindingSource, "DataBindings", true));
            this.textBoxPar1.Location = new System.Drawing.Point(92, 46);
            this.textBoxPar1.Name = "textBoxPar1";
            this.textBoxPar1.Size = new System.Drawing.Size(100, 20);
            this.textBoxPar1.TabIndex = 0;
            // 
            // textBoxPar2
            // 
            this.textBoxPar2.Location = new System.Drawing.Point(92, 72);
            this.textBoxPar2.Name = "textBoxPar2";
            this.textBoxPar2.Size = new System.Drawing.Size(100, 20);
            this.textBoxPar2.TabIndex = 1;
            // 
            // textBoxPar3
            // 
            this.textBoxPar3.Location = new System.Drawing.Point(92, 98);
            this.textBoxPar3.Name = "textBoxPar3";
            this.textBoxPar3.Size = new System.Drawing.Size(100, 20);
            this.textBoxPar3.TabIndex = 2;
            // 
            // textBoxPar4
            // 
            this.textBoxPar4.Location = new System.Drawing.Point(92, 124);
            this.textBoxPar4.Name = "textBoxPar4";
            this.textBoxPar4.Size = new System.Drawing.Size(100, 20);
            this.textBoxPar4.TabIndex = 3;
            // 
            // textBoxPar5
            // 
            this.textBoxPar5.Location = new System.Drawing.Point(92, 150);
            this.textBoxPar5.Name = "textBoxPar5";
            this.textBoxPar5.Size = new System.Drawing.Size(100, 20);
            this.textBoxPar5.TabIndex = 4;
            // 
            // labelPar2
            // 
            this.labelPar2.AutoSize = true;
            this.labelPar2.Location = new System.Drawing.Point(12, 75);
            this.labelPar2.Name = "labelPar2";
            this.labelPar2.Size = new System.Drawing.Size(64, 13);
            this.labelPar2.TabIndex = 6;
            this.labelPar2.Text = "Parameter 2";
            // 
            // labelPar1
            // 
            this.labelPar1.AutoSize = true;
            this.labelPar1.Location = new System.Drawing.Point(12, 49);
            this.labelPar1.Name = "labelPar1";
            this.labelPar1.Size = new System.Drawing.Size(64, 13);
            this.labelPar1.TabIndex = 7;
            this.labelPar1.Text = "Parameter 1";
            // 
            // labelPar3
            // 
            this.labelPar3.AutoSize = true;
            this.labelPar3.Location = new System.Drawing.Point(12, 101);
            this.labelPar3.Name = "labelPar3";
            this.labelPar3.Size = new System.Drawing.Size(64, 13);
            this.labelPar3.TabIndex = 8;
            this.labelPar3.Text = "Parameter 3";
            // 
            // labelPar5
            // 
            this.labelPar5.AutoSize = true;
            this.labelPar5.Location = new System.Drawing.Point(12, 157);
            this.labelPar5.Name = "labelPar5";
            this.labelPar5.Size = new System.Drawing.Size(64, 13);
            this.labelPar5.TabIndex = 9;
            this.labelPar5.Text = "Parameter 5";
            // 
            // labelPar4
            // 
            this.labelPar4.AutoSize = true;
            this.labelPar4.Location = new System.Drawing.Point(12, 127);
            this.labelPar4.Name = "labelPar4";
            this.labelPar4.Size = new System.Drawing.Size(64, 13);
            this.labelPar4.TabIndex = 10;
            this.labelPar4.Text = "Parameter 4";
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(92, 13);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(100, 21);
            this.comboBoxComPort.TabIndex = 11;
            this.comboBoxComPort.DropDown += new System.EventHandler(this.comboBoxComPort_DropDown);
            this.comboBoxComPort.SelectionChangeCommitted += new System.EventHandler(this.comboBoxComPort_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "COM-Port";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabelBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 527);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(754, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabelBar
            // 
            this.StatusLabelBar.Name = "StatusLabelBar";
            this.StatusLabelBar.Size = new System.Drawing.Size(56, 17);
            this.StatusLabelBar.Text = "StatusBar";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(244, 13);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(498, 485);
            this.listBox1.TabIndex = 15;
            // 
            // measuredParametersBindingSource
            // 
            this.measuredParametersBindingSource.DataSource = typeof(ic_project_2.MeasuredParameters);
            // 
            // form1BindingSource
            // 
            this.form1BindingSource.DataSource = typeof(ic_project_2.Form1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 549);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.labelPar4);
            this.Controls.Add(this.labelPar5);
            this.Controls.Add(this.labelPar3);
            this.Controls.Add(this.labelPar1);
            this.Controls.Add(this.labelPar2);
            this.Controls.Add(this.textBoxPar5);
            this.Controls.Add(this.textBoxPar4);
            this.Controls.Add(this.textBoxPar3);
            this.Controls.Add(this.textBoxPar2);
            this.Controls.Add(this.textBoxPar1);
            this.Name = "Form1";
            this.Text = "ComPort Communicator";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.measuredParametersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.form1BindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPar1;
        private System.Windows.Forms.TextBox textBoxPar2;
        private System.Windows.Forms.TextBox textBoxPar3;
        private System.Windows.Forms.TextBox textBoxPar4;
        private System.Windows.Forms.TextBox textBoxPar5;
        private System.Windows.Forms.Label labelPar2;
        private System.Windows.Forms.Label labelPar1;
        private System.Windows.Forms.Label labelPar3;
        private System.Windows.Forms.Label labelPar5;
        private System.Windows.Forms.Label labelPar4;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabelBar;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.BindingSource measuredParametersBindingSource;
        private System.Windows.Forms.BindingSource form1BindingSource;
    }
}


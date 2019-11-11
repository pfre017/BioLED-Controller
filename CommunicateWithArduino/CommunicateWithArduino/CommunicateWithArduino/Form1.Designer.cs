namespace CommunicateWithArduino
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.comPort = new System.Windows.Forms.ComboBox();
			this.baudRate = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnConnect = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.sendBtn = new System.Windows.Forms.Button();
			this.sendText = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.receiveText = new System.Windows.Forms.TextBox();
			this.clrBtn = new System.Windows.Forms.Button();
			this.demoBtn = new System.Windows.Forms.Button();
			this.revBtn = new System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.comPort);
			this.groupBox2.Controls.Add(this.baudRate);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.btnConnect);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(201, 99);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Connection Manager";
			// 
			// comPort
			// 
			this.comPort.FormattingEnabled = true;
			this.comPort.Location = new System.Drawing.Point(71, 18);
			this.comPort.Name = "comPort";
			this.comPort.Size = new System.Drawing.Size(121, 21);
			this.comPort.TabIndex = 6;
			// 
			// baudRate
			// 
			this.baudRate.FormattingEnabled = true;
			this.baudRate.Location = new System.Drawing.Point(71, 43);
			this.baudRate.Name = "baudRate";
			this.baudRate.Size = new System.Drawing.Size(121, 21);
			this.baudRate.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Baud rate: ";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "COM:";
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(9, 72);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(183, 21);
			this.btnConnect.TabIndex = 1;
			this.btnConnect.Text = "Connect!";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.sendBtn);
			this.groupBox1.Controls.Add(this.sendText);
			this.groupBox1.Location = new System.Drawing.Point(220, 13);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(255, 74);
			this.groupBox1.TabIndex = 6;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Data to send";
			// 
			// sendBtn
			// 
			this.sendBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.sendBtn.Location = new System.Drawing.Point(174, 43);
			this.sendBtn.Name = "sendBtn";
			this.sendBtn.Size = new System.Drawing.Size(75, 23);
			this.sendBtn.TabIndex = 1;
			this.sendBtn.Text = "Send";
			this.sendBtn.UseVisualStyleBackColor = true;
			this.sendBtn.Click += new System.EventHandler(this.sendBtn_Click);
			// 
			// sendText
			// 
			this.sendText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.sendText.Location = new System.Drawing.Point(7, 17);
			this.sendText.Name = "sendText";
			this.sendText.Size = new System.Drawing.Size(242, 20);
			this.sendText.TabIndex = 0;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.receiveText);
			this.groupBox3.Location = new System.Drawing.Point(12, 118);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(463, 268);
			this.groupBox3.TabIndex = 7;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Received data";
			// 
			// receiveText
			// 
			this.receiveText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.receiveText.Location = new System.Drawing.Point(6, 19);
			this.receiveText.Multiline = true;
			this.receiveText.Name = "receiveText";
			this.receiveText.ReadOnly = true;
			this.receiveText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.receiveText.Size = new System.Drawing.Size(451, 243);
			this.receiveText.TabIndex = 0;
			// 
			// clrBtn
			// 
			this.clrBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.clrBtn.Location = new System.Drawing.Point(12, 392);
			this.clrBtn.Name = "clrBtn";
			this.clrBtn.Size = new System.Drawing.Size(51, 23);
			this.clrBtn.TabIndex = 8;
			this.clrBtn.Text = "Clear";
			this.clrBtn.UseVisualStyleBackColor = true;
			this.clrBtn.Click += new System.EventHandler(this.clrBtn_Click);
			// 
			// demoBtn
			// 
			this.demoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.demoBtn.Location = new System.Drawing.Point(69, 392);
			this.demoBtn.Name = "demoBtn";
			this.demoBtn.Size = new System.Drawing.Size(49, 23);
			this.demoBtn.TabIndex = 9;
			this.demoBtn.Text = "Demo";
			this.demoBtn.UseVisualStyleBackColor = true;
			this.demoBtn.Click += new System.EventHandler(this.demoBtn_Click);
			// 
			// revBtn
			// 
			this.revBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.revBtn.Location = new System.Drawing.Point(124, 392);
			this.revBtn.Name = "revBtn";
			this.revBtn.Size = new System.Drawing.Size(53, 23);
			this.revBtn.TabIndex = 10;
			this.revBtn.Text = "Reverse";
			this.revBtn.UseVisualStyleBackColor = true;
			this.revBtn.Click += new System.EventHandler(this.revBtn_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 427);
			this.Controls.Add(this.revBtn);
			this.Controls.Add(this.demoBtn);
			this.Controls.Add(this.clrBtn);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Name = "Form1";
			this.Text = "Serial communication with Arduino";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.ComboBox baudRate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comPort;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button sendBtn;
		private System.Windows.Forms.TextBox sendText;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox receiveText;
		private System.Windows.Forms.Button clrBtn;
		private System.Windows.Forms.Button demoBtn;
		private System.Windows.Forms.Button revBtn;
	}
}


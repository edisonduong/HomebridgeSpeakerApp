namespace HomebridgeSpeakerApp
{
    partial class HomeBridgeSpeakerApp
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            logRichTextBox = new RichTextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            button2 = new Button();
            button1 = new Button();
            btnSave = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txtAccessoryID = new TextBox();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            txtHomebridgePort = new TextBox();
            txtHomebridgeIP = new TextBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // logRichTextBox
            // 
            logRichTextBox.BackColor = Color.FromArgb(10, 10, 10);
            logRichTextBox.Dock = DockStyle.Fill;
            logRichTextBox.Font = new Font("Consolas", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            logRichTextBox.Location = new Point(3, 3);
            logRichTextBox.Name = "logRichTextBox";
            logRichTextBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            logRichTextBox.Size = new Size(767, 396);
            logRichTextBox.TabIndex = 1;
            logRichTextBox.Text = "";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(781, 430);
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(logRichTextBox);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(773, 402);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Logs";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(btnSave);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(txtAccessoryID);
            tabPage2.Controls.Add(txtPassword);
            tabPage2.Controls.Add(txtUsername);
            tabPage2.Controls.Add(txtHomebridgePort);
            tabPage2.Controls.Add(txtHomebridgeIP);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(773, 402);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Settings";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(637, 21);
            button2.Name = "button2";
            button2.Size = new Size(128, 23);
            button2.TabIndex = 23;
            button2.Text = "Log All Accessories";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.ForeColor = Color.Red;
            button1.Location = new Point(201, 240);
            button1.Name = "button1";
            button1.Size = new Size(181, 23);
            button1.TabIndex = 22;
            button1.Text = "Reset Settings";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(8, 240);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(181, 23);
            btnSave.TabIndex = 21;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(8, 179);
            label5.Name = "label5";
            label5.Size = new Size(74, 15);
            label5.TabIndex = 20;
            label5.Text = "Accessory ID";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 135);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 19;
            label4.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(8, 91);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 18;
            label3.Text = "Username";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 47);
            label2.Name = "label2";
            label2.Size = new Size(99, 15);
            label2.TabIndex = 17;
            label2.Text = "Homebridge Port";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 3);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 16;
            label1.Text = "Homebridge IP";
            // 
            // txtAccessoryID
            // 
            txtAccessoryID.Location = new Point(8, 197);
            txtAccessoryID.Name = "txtAccessoryID";
            txtAccessoryID.Size = new Size(374, 23);
            txtAccessoryID.TabIndex = 15;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(8, 153);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '•';
            txtPassword.Size = new Size(374, 23);
            txtPassword.TabIndex = 14;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(8, 109);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(374, 23);
            txtUsername.TabIndex = 13;
            // 
            // txtHomebridgePort
            // 
            txtHomebridgePort.Location = new Point(8, 65);
            txtHomebridgePort.Name = "txtHomebridgePort";
            txtHomebridgePort.Size = new Size(374, 23);
            txtHomebridgePort.TabIndex = 12;
            // 
            // txtHomebridgeIP
            // 
            txtHomebridgeIP.Location = new Point(8, 21);
            txtHomebridgeIP.Name = "txtHomebridgeIP";
            txtHomebridgeIP.Size = new Size(374, 23);
            txtHomebridgeIP.TabIndex = 11;
            // 
            // HomeBridgeSpeakerApp
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(781, 430);
            Controls.Add(tabControl1);
            ForeColor = Color.Black;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "HomeBridgeSpeakerApp";
            Text = "Home Bridge Speaker App";
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox logRichTextBox;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button btnSave;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox txtAccessoryID;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private TextBox txtHomebridgePort;
        private TextBox txtHomebridgeIP;
        private Button button1;
        private Button button2;
    }
}

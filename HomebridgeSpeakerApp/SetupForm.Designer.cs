namespace HomebridgeSpeakerApp
{
    partial class SetupForm
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
            txtHomebridgeIP = new TextBox();
            txtHomebridgePort = new TextBox();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            txtAccessoryID = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnSave = new Button();
            SuspendLayout();
            // 
            // txtHomebridgeIP
            // 
            txtHomebridgeIP.Location = new Point(12, 27);
            txtHomebridgeIP.Name = "txtHomebridgeIP";
            txtHomebridgeIP.Size = new Size(240, 23);
            txtHomebridgeIP.TabIndex = 0;
            // 
            // txtHomebridgePort
            // 
            txtHomebridgePort.Location = new Point(12, 71);
            txtHomebridgePort.Name = "txtHomebridgePort";
            txtHomebridgePort.Size = new Size(240, 23);
            txtHomebridgePort.TabIndex = 1;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(12, 115);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(240, 23);
            txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(12, 159);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '•';
            txtPassword.Size = new Size(240, 23);
            txtPassword.TabIndex = 3;
            // 
            // txtAccessoryID
            // 
            txtAccessoryID.Location = new Point(12, 203);
            txtAccessoryID.Name = "txtAccessoryID";
            txtAccessoryID.Size = new Size(240, 23);
            txtAccessoryID.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 5;
            label1.Text = "Homebridge IP";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 53);
            label2.Name = "label2";
            label2.Size = new Size(99, 15);
            label2.TabIndex = 6;
            label2.Text = "Homebridge Port";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 97);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 7;
            label3.Text = "Username";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 141);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 8;
            label4.Text = "Password";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 185);
            label5.Name = "label5";
            label5.Size = new Size(74, 15);
            label5.TabIndex = 9;
            label5.Text = "Accessory ID";
            // 
            // btnSave
            // 
            btnSave.Location = new Point(12, 246);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(240, 23);
            btnSave.TabIndex = 10;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // SetupForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(264, 281);
            Controls.Add(btnSave);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(txtAccessoryID);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(txtHomebridgePort);
            Controls.Add(txtHomebridgeIP);
            Name = "SetupForm";
            Text = "SetupForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtHomebridgeIP;
        private TextBox txtHomebridgePort;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtAccessoryID;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btnSave;
    }
}
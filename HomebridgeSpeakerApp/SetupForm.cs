using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace HomebridgeSpeakerApp
{
    public partial class SetupForm : Form
    {
        public string HomebridgeIP { get; private set; }
        public string HomebridgePort { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string AccessoryID { get; private set; }

        public SetupForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            HomebridgeIP = txtHomebridgeIP.Text;
            HomebridgePort = txtHomebridgePort.Text;
            Username = txtUsername.Text;
            Password = txtPassword.Text;
            AccessoryID = txtAccessoryID.Text;

            if (string.IsNullOrWhiteSpace(HomebridgeIP) || string.IsNullOrWhiteSpace(HomebridgePort) ||
                string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(AccessoryID))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

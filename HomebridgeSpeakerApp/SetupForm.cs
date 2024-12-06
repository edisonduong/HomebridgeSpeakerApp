using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
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
            btnFetchAccessories.Click += BtnFetchAccessories_Click;
        }

        private async void BtnFetchAccessories_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHomebridgeIP.Text) ||
                string.IsNullOrWhiteSpace(txtHomebridgePort.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill out all fields before fetching accessories.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnFetchAccessories.Enabled = false;
            btnFetchAccessories.Text = "Fetching...";

            try
            {
                var accessories = await FetchAccessoriesAsync(txtHomebridgeIP.Text, txtHomebridgePort.Text, txtUsername.Text, txtPassword.Text);
                PopulateAccessoryDropdown(accessories);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching accessories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnFetchAccessories.Enabled = true;
                btnFetchAccessories.Text = "Fetch Accessories";
            }
        }

        private async Task<List<(string id, string name)>> FetchAccessoriesAsync(string ip, string port, string username, string password)
        {
            using var apiClient = new ApiClient();
            string url = $"http://{ip}:{port}/api/accessories";
            string token = await GetTokenAsync(apiClient, ip, port, username, password);
            var headers = new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };
            string response = await apiClient.GetAsync(url, headers);

            return ParseAccessories(response);
        }

        private async Task<string> GetTokenAsync(ApiClient apiClient, string ip, string port, string username, string password)
        {
            string url = $"http://{ip}:{port}/api/auth/login";
            string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

            string response = await apiClient.PostAsync(url, jsonData);
            using var jsonDoc = JsonDocument.Parse(response);
            return jsonDoc.RootElement.GetProperty("access_token").GetString();
        }

        private List<(string id, string name)> ParseAccessories(string response)
        {
            var accessories = new List<(string id, string name)>();
            using var jsonDoc = JsonDocument.Parse(response);

            foreach (var accessory in jsonDoc.RootElement.EnumerateArray())
            {
                string id = accessory.GetProperty("uniqueId").ToString();
                string name = accessory.GetProperty("serviceName").GetString();
                accessories.Add((id, name));
            }

            return accessories;
        }

        private void PopulateAccessoryDropdown(List<(string id, string name)> accessories)
        {
            cboAccessories.Items.Clear();

            var bindingSource = new BindingSource();

            foreach (var accessory in accessories)
            {
                bindingSource.Add(new KeyValuePair<string, string>(accessory.id, accessory.name));
            }

            cboAccessories.DataSource = bindingSource;
            cboAccessories.DisplayMember = "Value";
            cboAccessories.ValueMember = "Key";

            if (cboAccessories.Items.Count > 0)
            {
                cboAccessories.SelectedIndex = 0;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            HomebridgeIP = txtHomebridgeIP.Text;
            HomebridgePort = txtHomebridgePort.Text;
            Username = txtUsername.Text;
            Password = txtPassword.Text;

            if (cboAccessories.SelectedItem is KeyValuePair<string, string> selectedAccessory)
            {
                AccessoryID = selectedAccessory.Key;
            }
            else
            {
                MessageBox.Show("Please select an accessory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

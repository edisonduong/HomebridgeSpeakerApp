using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomebridgeSpeakerApp
{
    public partial class HomeBridgeSpeakerApp : Form
    {
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        private bool exitApplication = false;
        private string HOME_BRIDGE_IP;
        private string HOME_BRIDGE_PORT;
        private string USERNAME;
        private string PASSWORD;
        private string ACCESSORY_ID;
        private DateTime expirationDate = DateTime.UtcNow;
        private string bearerToken = null;
        private bool speakerStatus = false;
        private System.Timers.Timer autoFetchTimer;
        private const int AutoFetchInterval = 10000;

        public HomeBridgeSpeakerApp()
        {
            InitializeComponent();
            InitializeTrayIcon();
            ConfigureForm();

            if (!LoadOrCreateSettings())
            {
                MessageBox.Show("Unable to start application without valid settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            InitializeAutoFetchTimer();
            _ = InitializeSpeakerStatusAsync();
        }

        private void InitializeAutoFetchTimer()
        {
            autoFetchTimer = new System.Timers.Timer(AutoFetchInterval);
            autoFetchTimer.Elapsed += async (sender, e) => await AutoFetchStatusAsync();
            autoFetchTimer.AutoReset = true;
            autoFetchTimer.Start();
        }

        private async Task AutoFetchStatusAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ACCESSORY_ID))
                {
                    AppendLog("Accessory ID is not set. Skipping auto-fetch.", Color.Orange);
                    return;
                }

                if (!await IsTokenValidAsync()) return;

                var response = await GetSpeakerStatusResponseAsync();
                if (response != null)
                {
                    ParseSpeakerStatus(response);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error during auto-fetch: {ex.Message}", Color.Red);
            }
        }

        private bool LoadOrCreateSettings()
        {
            var credentials = SettingsManager.LoadCredentials();
            if (credentials.HasValue)
            {
                (HOME_BRIDGE_IP, HOME_BRIDGE_PORT, USERNAME, PASSWORD, ACCESSORY_ID) = credentials.Value;
                PopulateFieldsFromSettings();
                return true;
            }

            return ShowSetupFormAndSaveSettings();
        }

        private void PopulateFieldsFromSettings()
        {
            txtHomebridgeIP.Text = HOME_BRIDGE_IP;
            txtHomebridgePort.Text = HOME_BRIDGE_PORT;
            txtUsername.Text = USERNAME;
            txtPassword.Text = PASSWORD;
            txtAccessoryID.Text = ACCESSORY_ID;
        }

        private bool ShowSetupFormAndSaveSettings()
        {
            using (var setupForm = new SetupForm())
            {
                if (setupForm.ShowDialog() == DialogResult.OK)
                {
                    UpdateFieldsFromForm(setupForm);
                    SettingsManager.SaveCredentials(HOME_BRIDGE_IP, HOME_BRIDGE_PORT, USERNAME, PASSWORD, ACCESSORY_ID);
                    return true;
                }
            }
            return false;
        }

        private void UpdateFieldsFromForm(SetupForm setupForm)
        {
            HOME_BRIDGE_IP = setupForm.HomebridgeIP;
            HOME_BRIDGE_PORT = setupForm.HomebridgePort;
            USERNAME = setupForm.Username;
            PASSWORD = setupForm.Password;
            ACCESSORY_ID = setupForm.AccessoryID;
        }

        private void InitializeTrayIcon()
        {
            notifyIcon = new NotifyIcon
            {
                Text = "Homebridge Speaker App",
                Visible = true,
                ContextMenuStrip = CreateContextMenu()
            };
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
        }

        private ContextMenuStrip CreateContextMenu()
        {
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(CreateMenuItem("HomeBridge Speaker Controller", FontStyle.Bold, false));
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(CreateMenuItem("Toggle On", FontStyle.Regular, true, (sender, e) => ToggleSpeakers(true)));
            contextMenu.Items.Add(CreateMenuItem("Toggle Off", FontStyle.Regular, true, (sender, e) => ToggleSpeakers(false)));
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(CreateMenuItem("Show", FontStyle.Regular, true, ShowForm));
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(CreateMenuItem("Exit", FontStyle.Regular, true, ExitApplication));
            return contextMenu;
        }

        private ToolStripMenuItem CreateMenuItem(string text, FontStyle fontStyle, bool enabled, EventHandler clickHandler = null)
        {
            var menuItem = new ToolStripMenuItem(text)
            {
                Enabled = enabled,
                Font = new Font(contextMenu.Font, fontStyle)
            };

            if (clickHandler != null)
            {
                menuItem.Click += clickHandler;
            }

            return menuItem;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ToggleSpeakers(!speakerStatus);
            }
        }

        private async Task InitializeSpeakerStatusAsync()
        {
            if (!await IsTokenValidAsync()) return;

            var response = await GetSpeakerStatusResponseAsync();
            if (response != null)
            {
                ParseSpeakerStatus(response);
            }
        }

        private async Task<bool> IsTokenValidAsync()
        {
            try
            {
                if (bearerToken == null || expirationDate <= DateTime.Now.AddMinutes(1))
                {
                    await GetHomebridgeToken();
                }
                return true;
            }
            catch (Exception ex)
            {
                AppendLog($"Error: {ex.Message}", Color.Red);
                return false;
            }
        }

        private async Task<string> GetSpeakerStatusResponseAsync()
        {
            try
            {
                using var apiClient = new ApiClient();
                var headers = new Dictionary<string, string> { { "Authorization", $"Bearer {bearerToken}" } };
                string url = $"http://{HOME_BRIDGE_IP}:{HOME_BRIDGE_PORT}/api/accessories/{ACCESSORY_ID}";
                return await apiClient.GetAsync(url, headers);
            }
            catch (Exception ex)
            {
                AppendLog($"Error getting speaker status: {ex.Message}", Color.Red);
                return null;
            }
        }

        private void ParseSpeakerStatus(string response)
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(response);
                if (jsonDoc.RootElement.TryGetProperty("serviceCharacteristics", out JsonElement serviceCharacteristics))
                {
                    foreach (var characteristic in serviceCharacteristics.EnumerateArray())
                    {
                        if (characteristic.TryGetProperty("type", out JsonElement typeElement) &&
                            typeElement.GetString() == "On" &&
                            characteristic.TryGetProperty("value", out JsonElement valueElement))
                        {
                            speakerStatus = valueElement.GetInt32() == 1;
                            UpdateNotifyIcon();
                            AppendLog($"Characteristic 'On' value: {valueElement.GetInt32()}", Color.LightBlue);
                            break;
                        }
                    }
                }
                else
                {
                    AppendLog("serviceCharacteristics array not found in the response.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error parsing response: {ex.Message}", Color.Red);

                var result = MessageBox.Show(
                    "Failed to parse speaker status. Do you want to reset the accessory ID?",
                    "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);

                if (result == DialogResult.Yes)
                {
                    ResetAccessoryID();
                }
            }
        }

        private void UpdateNotifyIcon()
        {
            if (InvokeRequired)
            {
                Invoke((System.Windows.Forms.MethodInvoker)UpdateNotifyIcon);
            }
            else
            {
                try
                {
                    string resourcePath = speakerStatus ? "HomebridgeSpeakerApp.switch-on.ico" : "HomebridgeSpeakerApp.switch-off.ico";
                    notifyIcon.Icon = GetEmbeddedIcon(resourcePath);
                    notifyIcon.Text = speakerStatus ? "Speakers On" : "Speakers Off";
                }
                catch (Exception ex)
                {
                    AppendLog($"Error updating notify icon: {ex.Message}", Color.Red);
                }
            }
        }

        private Icon GetEmbeddedIcon(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourcePath);
            if (stream == null)
            {
                AppendLog($"Resource '{resourcePath}' not found.", Color.Red);
                throw new Exception($"Resource '{resourcePath}' not found.");
            }

            try
            {
                return new Icon(stream);
            }
            catch (Exception ex)
            {
                AppendLog($"Error loading icon from resource '{resourcePath}': {ex.Message}", Color.Red);
                throw;
            }
        }

        private async void ToggleSpeakers(bool toggle)
        {
            try
            {
                if (!await IsTokenValidAsync()) return;

                using var apiClient = new ApiClient();
                int toggleValue = toggle ? 1 : 0;
                var headers = new Dictionary<string, string> { { "Authorization", $"Bearer {bearerToken}" } };
                string url = $"http://{HOME_BRIDGE_IP}:{HOME_BRIDGE_PORT}/api/accessories/{ACCESSORY_ID}";
                string jsonData = $"{{\"characteristicType\":\"On\",\"value\":\"{toggleValue}\"}}";

                // Send the request and get the full response
                var response = await apiClient.PutAsync(url, jsonData, headers);

                // Check if the response status is Unauthorized (401)
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    AppendLog("Token expired. Attempting to renew...", Color.Orange);
                    await GetHomebridgeToken();

                    // Retry the request with the new token
                    headers["Authorization"] = $"Bearer {bearerToken}";
                    response = await apiClient.PutAsync(url, jsonData, headers);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        AppendLog("Failed to toggle speakers: Unauthorized after token renewal.", Color.Red);
                        return;
                    }
                }

                if (response.IsSuccessStatusCode)
                {
                    AppendLog($"Toggled speakers: {toggle}", Color.Chartreuse);
                    speakerStatus = toggle;
                    UpdateNotifyIcon();
                }
                else
                {
                    AppendLog($"Error toggling speakers: {response.ReasonPhrase}", Color.Red);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error toggling speakers: {ex.Message}", Color.Red);
            }
        }

        private async Task GetHomebridgeToken()
        {
            try
            {
                using var apiClient = new ApiClient();
                string url = $"http://{HOME_BRIDGE_IP}:{HOME_BRIDGE_PORT}/api/auth/login";
                string jsonData = $"{{\"username\":\"{USERNAME}\",\"password\":\"{PASSWORD}\"}}";
                string response = await apiClient.PostAsync(url, jsonData);
                var jsonDoc = JsonDocument.Parse(response);
                bearerToken = jsonDoc.RootElement.GetProperty("access_token").GetString();
                SetExpiration(jsonDoc.RootElement.GetProperty("expires_in").GetInt32());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting token: {ex.Message}");
            }
        }

        private async Task FetchAllAccessoriesAsync()
        {
            try
            {
                if (!await IsTokenValidAsync()) return;

                using var apiClient = new ApiClient();
                var headers = new Dictionary<string, string> { { "Authorization", $"Bearer {bearerToken}" } };
                string url = $"http://{HOME_BRIDGE_IP}:{HOME_BRIDGE_PORT}/api/accessories";
                string response = await apiClient.GetAsync(url, headers);

                AppendLog("Fetched all accessories successfully.", Color.Chartreuse);
                ParseAndLogAccessories(response);
            }
            catch (Exception ex)
            {
                AppendLog($"Error fetching accessories: {ex.Message}", Color.Red);
            }
        }

        private void ParseAndLogAccessories(string response)
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(response);

                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    AppendLog("Accessories List:", Color.LightBlue);

                    foreach (var accessory in jsonDoc.RootElement.EnumerateArray())
                    {
                        int aid = accessory.GetProperty("aid").GetInt32();
                        string type = accessory.GetProperty("type").GetString();
                        string humanType = accessory.GetProperty("humanType").GetString();
                        string serviceName = accessory.GetProperty("serviceName").GetString();
                        string uniqueId = GetPropertyValue(accessory, "uniqueId");

                        AppendLog($"Accessory ID: {aid}, Name: {serviceName}, Type: {humanType}, Unique ID: {uniqueId}", Color.LightGreen);

                        if (accessory.TryGetProperty("accessoryInformation", out JsonElement accessoryInfo))
                        {
                            string manufacturer = GetPropertyValue(accessoryInfo, "Manufacturer");
                            string model = GetPropertyValue(accessoryInfo, "Model");
                            string name = GetPropertyValue(accessoryInfo, "Name");
                            string serialNumber = GetPropertyValue(accessoryInfo, "Serial Number");
                            string firmwareRevision = GetPropertyValue(accessoryInfo, "Firmware Revision");

                            AppendLog($"  Manufacturer: {manufacturer}, Model: {model}, Serial Number: {serialNumber}", Color.LightGreen);
                            AppendLog($"  Firmware Revision: {firmwareRevision}", Color.LightGreen);
                        }

                        if (accessory.TryGetProperty("serviceCharacteristics", out JsonElement characteristics))
                        {
                            AppendLog("  Characteristics:", Color.LightYellow);

                            foreach (var characteristic in characteristics.EnumerateArray())
                            {
                                string characteristicType = GetPropertyValue(characteristic, "type");
                                string description = GetPropertyValue(characteristic, "description");
                                string value = GetPropertyValue(characteristic, "value");
                                string format = GetPropertyValue(characteristic, "format");

                                AppendLog($"    {description} (Type: {characteristicType}, Format: {format}, Value: {value})", Color.White);
                            }
                        }

                        AppendLog("-----------------------------", Color.LightGray);
                    }
                }
                else
                {
                    AppendLog("The response is not an array of accessories.", Color.Orange);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"Error parsing accessories: {ex.Message}", Color.Red);
            }
        }

        private string GetPropertyValue(JsonElement element, string propertyName)
        {
            return element.TryGetProperty(propertyName, out JsonElement propertyValue)
                ? propertyValue.ToString()
                : "N/A";
        }

        private void SetExpiration(int secondsUntilExpiration)
        {
            expirationDate = DateTime.UtcNow.AddSeconds(secondsUntilExpiration);
        }

        private void ConfigureForm()
        {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            FormBorderStyle = FormBorderStyle.None;
            Opacity = 0;
            Load += (sender, e) => { Hide(); };
            FormClosed += (sender, e) => notifyIcon.Dispose();
        }

        private void ShowForm(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            Opacity = 1;
            ShowInTaskbar = true;
            FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void ExitApplication(object sender, EventArgs e)
        {
            exitApplication = true;
            notifyIcon.Visible = false;
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!exitApplication)
            {
                e.Cancel = true;
                Hide();
                ShowInTaskbar = false;
                Opacity = 0;
                return;
            }
            if (autoFetchTimer != null)
            {
                autoFetchTimer.Stop();
                autoFetchTimer.Dispose();
            }
            base.OnFormClosing(e);
        }

        private void AppendLog(string message, Color messageColor)
        {
            if (logRichTextBox.InvokeRequired)
            {
                logRichTextBox.Invoke(new Action<string, Color>(AppendLog), message, messageColor);
            }
            else
            {
                logRichTextBox.SelectionStart = logRichTextBox.TextLength;
                logRichTextBox.SelectionLength = 0;
                logRichTextBox.SelectionColor = Color.Yellow;
                logRichTextBox.AppendText($"[{DateTime.Now:HH:mm:ss}]");
                logRichTextBox.SelectionStart = logRichTextBox.TextLength;
                logRichTextBox.SelectionColor = messageColor;
                logRichTextBox.AppendText($" {message}{Environment.NewLine}");
                logRichTextBox.SelectionStart = logRichTextBox.Text.Length;
                logRichTextBox.SelectionLength = 0;
                logRichTextBox.SelectionColor = logRichTextBox.ForeColor;
                logRichTextBox.SelectionStart = logRichTextBox.Text.Length;
                logRichTextBox.ScrollToCaret();
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            UpdateFieldsFromInputs();

            if (!AreFieldsValid())
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SettingsManager.SaveCredentials(HOME_BRIDGE_IP, HOME_BRIDGE_PORT, USERNAME, PASSWORD, ACCESSORY_ID);
            await InitializeSpeakerStatusAsync();
            bearerToken = null;

            MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }

        private void UpdateFieldsFromInputs()
        {
            HOME_BRIDGE_IP = txtHomebridgeIP.Text;
            HOME_BRIDGE_PORT = txtHomebridgePort.Text;
            USERNAME = txtUsername.Text;
            PASSWORD = txtPassword.Text;
            ACCESSORY_ID = txtAccessoryID.Text;
        }

        private bool AreFieldsValid()
        {
            return !(string.IsNullOrWhiteSpace(HOME_BRIDGE_IP) ||
                     string.IsNullOrWhiteSpace(HOME_BRIDGE_PORT) ||
                     string.IsNullOrWhiteSpace(USERNAME) ||
                     string.IsNullOrWhiteSpace(PASSWORD) ||
                     string.IsNullOrWhiteSpace(ACCESSORY_ID));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearSavedData();
        }

        private void ClearSavedData()
        {
            var result = MessageBox.Show("Are you sure you want to clear all saved data? This action cannot be undone.", "Confirm Clear Data", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                SettingsManager.ClearCredentials();
                MessageBox.Show("Data cleared successfully.", "Data Cleared", MessageBoxButtons.OK, MessageBoxIcon.None);
                LoadOrCreateSettings();
            }
        }

        private void ResetAccessoryID()
        {
            ACCESSORY_ID = string.Empty;
            txtAccessoryID.Text = string.Empty;

            SettingsManager.SaveCredentials(HOME_BRIDGE_IP, HOME_BRIDGE_PORT, USERNAME, PASSWORD, ACCESSORY_ID);
            AppendLog("Accessory ID has been cleared. Please reconfigure.", Color.Orange);

            MessageBox.Show("Accessory ID has been reset. Please enter a new ID and save.",
                            "Accessory Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Prompt for new Accessory ID
            if (string.IsNullOrWhiteSpace(ACCESSORY_ID))
            {
                MessageBox.Show("Accessory ID is not set. Please configure a new Accessory ID.", "Configuration Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowSetupFormAndSaveSettings();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FetchAllAccessoriesAsync();
        }
    }

    public static class SettingsManager
    {
        public static (string homebridgeIP, string homebridgePort, string username, string password, string accessoryID)? LoadCredentials()
        {
            return SecureData.LoadCredentials();
        }

        public static void SaveCredentials(string homebridgeIP, string homebridgePort, string username, string password, string accessoryID)
        {
            SecureData.SaveCredentials(homebridgeIP, homebridgePort, username, password, accessoryID);
        }

        public static void ClearCredentials()
        {
            SecureData.DeleteCredentials();
        }
    }
}

using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Collections.Generic;

namespace ServerInfoApp
{
    public partial class Form1 : Form
    {
        private Label titleLabel;
        private Label footerLabel;
        private Button infoButton;
        private Button diskInfoButton;
        private Button networkInfoButton;
        private Button installedProgramsButton;
        private Button exportButton;
        private TextBox infoTextBox;
        private TextBox diskInfoTextBox;
        private TextBox networkInfoTextBox;
        private TextBox installedProgramsTextBox;
        private Panel separatorPanel;

        public Form1()
        {
            InitializeComponent();

            // Lock window size and design
            this.Text = "SSFCU DR Documentation Server Info";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(10, 25, 74); // Dark Navy Blue
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.Size = new Size(750, 700); // Increased height for spacing
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Title Label
            titleLabel = new Label
            {
                Text = "SSFCU DR Documentation Server Info",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point((this.ClientSize.Width - 500) / 2, 10)
            };
            this.Controls.Add(titleLabel);

            // Footer Label
            footerLabel = new Label
            {
                Text = "SSFCU DR Server Info App | Design by Noe Tovar-MBA 2025",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.White,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            this.Controls.Add(footerLabel);

            // Separator Panel
            separatorPanel = new Panel
            {
                Height = 1,
                Width = this.ClientSize.Width - 60,
                BackColor = Color.DimGray
            };
            this.Controls.Add(separatorPanel);

            // Buttons
            infoButton = CreateStyledButton("Get Server Information", 50, 50);
            infoButton.Click += InfoButton_Click;

            diskInfoButton = CreateStyledButton("Get Disk Information", 50, 150);
            diskInfoButton.Click += DiskInfoButton_Click;

            networkInfoButton = CreateStyledButton("Get Network Information", 50, 250);
            networkInfoButton.Click += NetworkInfoButton_Click;

            installedProgramsButton = CreateStyledButton("Get Installed Programs", 50, 350);
            installedProgramsButton.Click += InstalledProgramsButton_Click;

            // Export (Print) Button
            exportButton = CreateStyledButton("Print All Info", (this.ClientSize.Width - 200) / 2, 540);
            exportButton.Size = new Size(200, 45);
            exportButton.Anchor = AnchorStyles.Bottom;
            exportButton.Click += ExportButton_Click;

            // Textboxes
            infoTextBox = CreateTextBox(300, 50);
            diskInfoTextBox = CreateTextBox(300, 150);
            networkInfoTextBox = CreateTextBox(300, 250);
            installedProgramsTextBox = CreateTextBox(300, 350);

            // Add all controls
            this.Controls.AddRange(new Control[]
            {
                infoButton, diskInfoButton, networkInfoButton, installedProgramsButton,
                exportButton,
                infoTextBox, diskInfoTextBox, networkInfoTextBox, installedProgramsTextBox
            });

            this.Load += (s, e) => CenterElements();
            this.Resize += (s, e) => CenterElements();
        }

        private void CenterElements()
        {
            titleLabel.Left = (this.ClientSize.Width - titleLabel.Width) / 2;

            exportButton.Left = (this.ClientSize.Width - exportButton.Width) / 2;
            exportButton.Top = this.ClientSize.Height - 140;

            separatorPanel.Left = 30;
            separatorPanel.Top = exportButton.Bottom + 10;
            separatorPanel.Width = this.ClientSize.Width - 60;

            footerLabel.Left = (this.ClientSize.Width - footerLabel.Width) / 2;
            footerLabel.Top = separatorPanel.Bottom + 10;
        }

        private Button CreateStyledButton(string text, int x, int y)
        {
            return new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(200, 50),
                Location = new Point(x, y),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.MintCream,
                FlatAppearance = { BorderColor = Color.Black, BorderSize = 2 }
            };
        }

        private TextBox CreateTextBox(int x, int y)
        {
            return new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new Size(400, 80),
                Location = new Point(x, y),
                Anchor = AnchorStyles.Top
            };
        }

        // Event Handlers
        private void InfoButton_Click(object sender, EventArgs e)
        {
            string cpuInfo = GetCPUInfo();
            string ramInfo = GetRAMInfo();
            string osInfo = GetOSInfo();
            string hostName = Environment.MachineName;
            string userName = Environment.UserName;
            string domainName = Environment.UserDomainName;
            string productKey = GetWindowsProductKey();

            infoTextBox.Text = $"Host Name: {hostName}\n\n" +
                               $"User Name: {userName}\n\n" +
                               $"Domain Name: {domainName}\n\n" +
                               $"CPU:\n{cpuInfo}\n\n" +
                               $"RAM:\n{ramInfo}\n\n" +
                               $"Operating System:\n{osInfo}\n\n" +
                               $"Product Key: {productKey}";
        }

        private void DiskInfoButton_Click(object sender, EventArgs e)
        {
            diskInfoTextBox.Text = GetDiskInfo();
        }

        private void NetworkInfoButton_Click(object sender, EventArgs e)
        {
            networkInfoTextBox.Text = GetNetworkInfo();
        }

        private void InstalledProgramsButton_Click(object sender, EventArgs e)
        {
            installedProgramsTextBox.Text = GetInstalledPrograms();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = $"ServerInformation_{timestamp}.txt";

            string output = "=== SERVER INFORMATION ===\n" + infoTextBox.Text +
                            "\n\n=== DISK INFORMATION ===\n" + diskInfoTextBox.Text +
                            "\n\n=== NETWORK INFORMATION ===\n" + networkInfoTextBox.Text +
                            "\n\n=== INSTALLED PROGRAMS ===\n" + installedProgramsTextBox.Text +
                            "\n\n------------------------------------------\n" +
                            "SSFCU DR Documentation Server information application created by Noe Tovar-MBA 2025";

            File.WriteAllText(filename, output);
            MessageBox.Show($"Information saved to {filename}");
        }

        // System Info Helpers
        private string GetCPUInfo()
        {
            string cpuBaseSpeed = "", cpuCores = "", logicalProcessors = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                cpuBaseSpeed = $"{obj["MaxClockSpeed"]} MHz";
                cpuCores = obj["NumberOfCores"].ToString();
                logicalProcessors = obj["NumberOfLogicalProcessors"].ToString();
            }
            return $"Base Speed: {cpuBaseSpeed}\nCores: {cpuCores}\nLogical Processors: {logicalProcessors}";
        }

        private string GetRAMInfo()
        {
            string ram = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_ComputerSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                double totalRam = Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1024 * 1024 * 1024), 2);
                ram = $"{totalRam} GB";
            }
            return ram;
        }

        private string GetOSInfo()
        {
            string osName = "", osVersion = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                osName = obj["Caption"].ToString();
                osVersion = obj["Version"].ToString();
            }
            return $"{osName} (Version: {osVersion})";
        }

        private string GetDiskInfo()
        {
            string diskInfo = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_LogicalDisk");
            foreach (ManagementObject obj in searcher.Get())
            {
                string volumeName = obj["VolumeName"]?.ToString() ?? "Unnamed Volume";
                string volumeType = obj["Description"].ToString();
                double totalSize = Math.Round(Convert.ToDouble(obj["Size"]) / (1024 * 1024 * 1024), 2);
                double freeSpace = Math.Round(Convert.ToDouble(obj["FreeSpace"]) / (1024 * 1024 * 1024), 2);
                double percentFree = Math.Round((freeSpace / totalSize) * 100, 2);

                diskInfo += $"Volume: {volumeName}\n" +
                            $"Type: {volumeType}\n" +
                            $"Total Size: {totalSize} GB\n" +
                            $"Free Space: {freeSpace} GB\n" +
                            $"Percent Free: {percentFree}%\n\n";
            }
            return diskInfo.TrimEnd('\n');
        }

        private string GetNetworkInfo()
        {
            string ipAddress = "", subnetMask = "", gateway = "", dnsServers = "", uuid = "";
            foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipAddress = ip.Address.ToString();
                            subnetMask = ip.IPv4Mask.ToString();
                            break;
                        }
                    }
                    foreach (GatewayIPAddressInformation gatewayAddr in ni.GetIPProperties().GatewayAddresses)
                    {
                        gateway = gatewayAddr.Address.ToString();
                    }
                    foreach (IPAddress dns in ni.GetIPProperties().DnsAddresses)
                    {
                        if (dns.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            dnsServers += $"{dns}\n";
                        }
                    }
                    uuid = ni.Id;
                    break;
                }
            }
            return $"IP Address: {ipAddress}\n" +
                   $"Subnet Mask: {subnetMask}\n" +
                   $"Gateway: {gateway}\n" +
                   $"DNS Servers:\n{dnsServers.TrimEnd('\n')}\n" +
                   $"UUID: {uuid}";
        }

        private string GetWindowsProductKey()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from SoftwareLicensingService");
                foreach (ManagementObject obj in searcher.Get())
                {
                    return obj["OA3xOriginalProductKey"]?.ToString() ?? "Product Key not found";
                }
                return "Product Key not found";
            }
            catch (Exception ex)
            {
                return $"Error retrieving Product Key: {ex.Message}";
            }
        }

        private string GetInstalledPrograms()
        {
            var programNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string[] registryKeys = {
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (string keyPath in registryKeys)
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(keyPath))
                {
                    if (key == null) continue;

                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                        {
                            string displayName = subkey?.GetValue("DisplayName") as string;
                            if (!string.IsNullOrWhiteSpace(displayName))
                            {
                                programNames.Add(displayName);
                            }
                        }
                    }
                }
            }

            var sortedPrograms = programNames.OrderBy(p => p).ToList();
            return sortedPrograms.Count > 0 ? string.Join("\n", sortedPrograms) : "No installed programs found.";
        }
    }
}

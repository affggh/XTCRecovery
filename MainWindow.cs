using AntdUI;
using System.Diagnostics;
using System.IO.Compression;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace XTCRecovery
{
    public partial class MainWindow : AntdUI.Window
    {
        private System.Windows.Forms.Timer progressTimer;
        private float progressValue = 0;

        private Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;

        private XTCRecovery_Helper helper = new XTCRecovery_Helper();
        private Dictionary<string, Dictionary<string, string>>? queryData;

        private string comPort = "";
        private bool connectState = false;

        private bool forceLocal = false;
        private string localDir = "";

        public MainWindow()
        {
            InitializeComponent();

            this.Opacity = 0; // Hide window until we load all widgets

            progressTimer = new System.Windows.Forms.Timer();
            progressTimer.Interval = 500; // ÿ 100ms ����һ��
            progressTimer.Tick += progressTimer_Tick;

            ProgressBar.Value = progressValue;

            windowBar1.SubText = $"Version: {assemblyVersion}";

            var tooltip = new AntdUI.TooltipComponent();
            tooltip.ArrowAlign = TAlign.Bottom;
            tooltip.SetTip(refreshButton, "ˢ���豸��ˢ�밴ť����״̬");
            tooltip.SetTip(InfoButton, "����");
            tooltip.SetTip(button3, "��������ˢ���ͺ��б�");
            tooltip.SetTip(button2, "С�����ˢ��������/����");

            // bind model and version select
            modelSelect.SelectedIndexChanged += ChangeVersionIfModelSelected;

            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;

            flashButton.Click += FlashButton_Click1;

            uploadDragger.DragChanged += UploadDragger_DragChanged;
            uploadDragger.Click += UploadDragger_Click;

            this.Load += MainWindow_Load;
        }

        private void UploadDragger_DragDrop(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void UploadDragger_Click(object sender, EventArgs e)
        {
            // ����һ���ļ��жԻ���ʵ��
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // �����ļ��Ի��������
            openFileDialog.Filter = "rawprogram0.xml | rawprogram0.xml";
            openFileDialog.Title = "ѡ��ˢ�����е�rawprogram0.xml"; // �Ի������
            openFileDialog.Multiselect = false; // �������ѡ�ļ�

            // ��ʾ�ļ��Ի��򲢼���û��Ƿ�ѡ�����ļ�
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // ��ȡ�û�ѡ����ļ�·��
                string[] filePaths = openFileDialog.FileNames;
                foreach (string path in filePaths)
                {
                    //showMsg("File Input:", path, TType.Info);
                    bool result = await Task.Run(() => helper.Xor_file(path));

                    if (result)
                    {
                        log($"ǿ��ʹ�ñ���Ŀ¼ ... [{path}]");
                        modelSelect.Enabled = false;
                        versionSelect.Enabled = false;
                        forceLocal = true;
                        downloadButton.Enabled = false;
                        localDir = Directory.GetParent(path).FullName;
                    }

                }
            }
        }

        private async void UploadDragger_DragChanged(object sender, AntdUI.UploadDragger.StringsEventArgs e)
        {
            string[] filePaths = e.Value;
            foreach (string path in filePaths)
            {
                bool result = await Task.Run(() => helper.Xor_file(path));
                if (result)
                {
                    log($"ǿ��ʹ�ñ���Ŀ¼ ... [{path}]");
                    modelSelect.Enabled = false;
                    versionSelect.Enabled = false;
                    forceLocal = true;
                    downloadButton.Enabled = false;
                    localDir = Directory.GetParent(path).FullName;
                }
            }
        }

        private async void FlashButton_Click1(object sender, EventArgs e)
        {
            log("��ʼˢ�� ...");

            string scriptPath = Path.Combine(helper.getPrebuiltDir(), "flash.bat");

            await RunFlashScriptAsync(scriptPath);
        }

        private Task RunFlashScriptAsync(string scriptFile)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (Process process = new Process()) {
                        process.StartInfo.FileName = scriptFile;
                        process.StartInfo.Arguments = $"{comPort} {Directory.GetParent(getDownloadPath()).FullName}";
                        process.StartInfo.UseShellExecute = false; // ��������Ϊ false �����ض������
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.CreateNoWindow = true; // ����ʾ�����д���


                        // �����׼���
                        process.OutputDataReceived += (s, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                log(e.Data);
                            }
                        };
                        process.ErrorDataReceived += (s, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                log(e.Data);
                            }
                        };

                        // ��������
                        process.Start();

                        // �첽��ȡ����ʹ�����
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        // �ȴ��������
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            showMsg("Error:", "������ˢ��ʧ����", TType.Error);
                        } else
                        {
                            showMsg("Success:", "ˢ��ɹ���", TType.Success);
                        }
                    }
                }
                catch (Exception ex)
                {
                    showMsg("Error:", "Error when running script:" + ex.Message, TType.Error);
                }

            }
            );
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText("343516728");
            showMsg("Info:", "QQ Group number 343516728 has been copied into your clip board\nnow you can paste and search it");
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var xorWindow = new XorWindow();
            xorWindow.StartPosition = FormStartPosition.CenterScreen;
            xorWindow.Show();
        }

        private async void Button3_Click(object sender, EventArgs e)
        {
            await LoadModelList();
        }

        private void log(string value)
        {
            if (LogInput.InvokeRequired)
            {
                LogInput.Invoke(new Action(() => { 
                    LogInput.AppendText(value + Environment.NewLine);
                    LogInput.ScrollToEnd();
                }));

            } else
            {
                LogInput.AppendText(value + Environment.NewLine);
                LogInput.ScrollToEnd();
            }
            
        }

        private void showMsg(string title, string value, TType type)
        {
            AntdUI.Notification.open(new AntdUI.Notification.Config(this, title, value, type, TAlignFrom.BR) // Top right
            {
                Padding = new Size(24, 20),
                CloseIcon = false,
                //Link = new Notification.ConfigLink("Link to...", () =>
                //{
                //    AntdUI.Message.info(this, "Welcome!");
                //    return true;
                //})
            });
        }

        private void showMsg(string title, string value)
        {
            TType type = TType.Info;
            showMsg(title, value, type);
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            await LoadModelList();
            // Load model list
            this.Opacity = 1;
            this.Show();
            DetectQualcommDeviceConnected();
        }

        private void ChangeVersionIfModelSelected(object sender, IntEventArgs e)
        {
            log("��⵽�޸����豸��ˢ�°汾��...\n");
            versionSelect.Items = [];

            if (modelSelect.SelectedIndex != -1)
            {
                if (queryData != null && modelSelect.SelectedValue != null)
                {
                    foreach (var version in queryData[modelSelect.SelectedValue.ToString()])
                    {
                        versionSelect.Items.Add(version.Key);
                        log($"��Ӱ汾: {version.Key}");
                    }
                    versionSelect.SelectedValue = versionSelect.Items[0];
                    versionSelect.SelectedIndex = 0;
                }
                else
                {
                    showMsg("Error", $"�Ҳ����ͺ�����Ӧ�İ汾�� : {modelSelect.SelectedValue}", TType.Error);
                }
            }
            else
            {
                log($"ѡ���˴���� Index: {modelSelect.SelectedIndex}");
            }
        }

        private async Task LoadModelList()
        {
            queryData = await helper.GetAvaliableDictFromOnline();

            // Clear model list
            modelSelect.Items = [];
            log("�����豸�б�...");
            if (queryData != null)
            {
                foreach (var model in queryData)
                {
                    modelSelect.Items.Add(model.Key);
                }

                if (modelSelect.Items.Count > 0)
                {
                    log("��ȡ���豸�ͺ��б�:");
                    foreach (var item in modelSelect.Items)
                    {
                        log($"{item.ToString()}");
                    }
                    log("���سɹ���");
                    //Ensure_Flash_Prepared();
                }
                else
                {
                    log("����ʧ�ܣ�");
                }
            }
            else
            {
                log("����ʧ�ܣ�\n");
                log(helper.GetLastErrorMessage());
            }
        }

        private void FlashButton_Click(object sender, EventArgs e)
        {
            FlashSteps.Current = 0;
            log("Flash Button Clicked!\n");
            progressTimer.Start();
        }

        private void progressTimer_Tick(object? sender, EventArgs e)
        {
            if (progressValue < 1)
            {
                progressValue += 0.2f; // ÿ������ 10
                ProgressBar.Value = progressValue;
                if (FlashSteps.Current != 2)
                {
                    FlashSteps.Current += 1;
                }
            }
            else
            {
                progressTimer.Stop(); // ֹͣ Timer

                // �ӳ� 1 ���ָ�
                System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ =>
                {
                    // �ص����߳����ý�����
                    Invoke(new Action(() =>
                    {
                        ProgressBar.Value = 0;
                        progressValue = 0;
                        FlashSteps.Current = 0;
                    }));
                });
            }
        }

        private void InfoButton_Click(object sender, EventArgs e)
        {
            AntdUI.Modal.open(
                new AntdUI.Modal.Config(this, "����...", "�������Ŀ��������������ƭ��affgghд��\n�������˾��������ú�дһ����\n���Ϻ���affggh��")
                {
                    CloseIcon = true,
                    Keyboard = false,
                    MaskClosable = false,
                }
                );
        }

        private bool DetectQualcommDeviceConnected()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity"))
                {
                    foreach (var device in searcher.Get())
                    {
                        string deviceName = device["Name"]?.ToString() ?? string.Empty;
                        string deviceID = device["DeviceID"]?.ToString() ?? string.Empty;

                        // Check for "Qualcomm" and "9008" in device name or ID
                        if (deviceName.Contains("Qualcomm") &&
                            deviceName.Contains("9008"))
                        {
                            log(deviceName);
                            log(deviceID);
                            log("��⵽�豸��");
                            deviceBadge.Text = "������";
                            deviceBadge.State = AntdUI.TState.Success;
                            connectState = true;
                            // Set com port
                            var pattern = @"\bCOM\d+\b";
                            Match match = Regex.Match(deviceName, pattern);
                            if (match.Success)
                            {
                                comPort = match.Value;
                                log($"�˿ڳɹ�����Ϊ: [{comPort}]");
                            }
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            connectState = false;
            comPort = "";
            deviceBadge.Text = "δ����";
            deviceBadge.State = AntdUI.TState.Warn;
            return false;
        }

        private void Ensure_Flash_Prepared()
        {
            var dpath = getDownloadPath();
            var rpath = Directory.GetParent(dpath).FullName; // research path
            if (modelSelect.SelectedIndex < 0)
            {
                return;
            }

            if (connectState)
            {
                if (File.Exists(Path.Combine(rpath, "rawprogram0.xml")))
                {
                    log("���������Ѿ�׼�����ˣ�");
                    FlashSteps.Current = 2;
                    flashButton.Enabled = true;
                    return;
                } else
                {
                    showMsg("Error:", "ָ��·���� :\n" + dpath + "\n������rawprogram0.xml\n��ȷ������һ����ˢ��Ŀ¼��", TType.Error);
                }
            } else
            {
                showMsg("Error:", "�豸δ����", TType.Error);
            }
            flashButton.Enabled = false;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            log("����ˢ���豸״̬...\n");
            DetectQualcommDeviceConnected();
            Ensure_Flash_Prepared();
            log("ˢ����ɣ�\n");

        }

        private string getDownloadPath()
        {
            if (forceLocal)
            {
                return Path.Combine(localDir, "rawprogram0.xml");
            }

            var romDir = helper.getRomDir();
            var deviceRomDir = Path.Combine(romDir, $"{modelSelect.SelectedValue}_{versionSelect.SelectedValue}");

            if (!Directory.Exists(deviceRomDir) && !deviceRomDir.EndsWith("_"))
            {
                Directory.CreateDirectory(deviceRomDir);
            }

            return Path.Combine(deviceRomDir, "rom.zip");
        }

        private void DownloadProgress_Update(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar.Value = (float)e.ProgressPercentage / 100;
            //log($"���ؽ���{e.ProgressPercentage}");
        }

        private void WebClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                showMsg("Error", " occurred: " + e.Error.Message, TType.Error);
            }
            else if (e.Cancelled)
            {
                showMsg("Info", "Cancled");
            }
            else
            {
                showMsg("Success", "File successfully download!", TType.Success);
            }

            // ���ð�ť״̬������UI�߼�
            ProgressBar.Value = 0;

            // Extract when download is donw
            var dpath = getDownloadPath();
            log("Extracting rom file ...");
            FlashSteps.Current = 1;
            Task.Run(() => {
                try
                {
                    ZipFile.ExtractToDirectory(dpath, Directory.GetParent(dpath).FullName);
                    showMsg("Sueecss:", "Successfully extracted compressed rom package !", TType.Success);
                    FlashSteps.Current = 2;
                }
                catch (Exception ex)
                {
                    showMsg("Error:", "Error while extract rom zip file cause:" + ex.Message, TType.Error);
                }
                downloadButton.Enabled = true;
                Ensure_Flash_Prepared();
            });

        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            if (modelSelect.SelectedIndex > 0 && modelSelect.SelectedValue != null)
            {
                if (queryData != null && versionSelect.SelectedValue != null)
                {
                    var url = queryData[modelSelect.SelectedValue.ToString()][versionSelect.SelectedValue.ToString()];

                    log($"Starging download from url: {url} ...");
                    // Check if already download
                    if (File.Exists(Path.Combine(Directory.GetParent(getDownloadPath()).FullName, "rawprogram0.xml")))
                    {
                        showMsg("Warning:", "���Rom�Ѿ������ˣ�����Ҫ�ظ����أ�", TType.Warn);
                        return;
                    }

                    downloadButton.Enabled = false;

                    WebClient webClient = new WebClient();

                    webClient.DownloadProgressChanged += DownloadProgress_Update;
                    webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

                    webClient.DownloadFileAsync(new Uri(url), getDownloadPath());

                } else
                {
                    showMsg("Error:", "Could not find download url cause query data not exist!", TType.Error);
                }
            } else
            {
                showMsg("Error:", "���Ƚ��ͺ���汾����ѡ���������ˣ�", TType.Error);
            }

        }

        
    }

    
}

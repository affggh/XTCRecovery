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
            progressTimer.Interval = 500; // 每 100ms 触发一次
            progressTimer.Tick += progressTimer_Tick;

            ProgressBar.Value = progressValue;

            windowBar1.SubText = $"Version: {assemblyVersion}";

            var tooltip = new AntdUI.TooltipComponent();
            tooltip.ArrowAlign = TAlign.Bottom;
            tooltip.SetTip(refreshButton, "刷新设备和刷入按钮允许状态");
            tooltip.SetTip(InfoButton, "关于");
            tooltip.SetTip(button3, "从网络上刷新型号列表");
            tooltip.SetTip(button2, "小天才线刷包异或加密/解密");

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
            // 创建一个文件夹对话框实例
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置文件对话框的属性
            openFileDialog.Filter = "rawprogram0.xml | rawprogram0.xml";
            openFileDialog.Title = "选择刷机包中的rawprogram0.xml"; // 对话框标题
            openFileDialog.Multiselect = false; // 不允许多选文件

            // 显示文件对话框并检查用户是否选择了文件
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                string[] filePaths = openFileDialog.FileNames;
                foreach (string path in filePaths)
                {
                    //showMsg("File Input:", path, TType.Info);
                    bool result = await Task.Run(() => helper.Xor_file(path));

                    if (result)
                    {
                        log($"强制使用本地目录 ... [{path}]");
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
                    log($"强制使用本地目录 ... [{path}]");
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
            log("开始刷入 ...");

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
                        process.StartInfo.UseShellExecute = false; // 必须设置为 false 才能重定向输出
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.CreateNoWindow = true; // 不显示命令行窗口


                        // 捕获标准输出
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

                        // 启动进程
                        process.Start();

                        // 异步读取输出和错误流
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        // 等待进程完成
                        process.WaitForExit();

                        if (process.ExitCode != 0)
                        {
                            showMsg("Error:", "看起来刷入失败了", TType.Error);
                        } else
                        {
                            showMsg("Success:", "刷入成功！", TType.Success);
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
            log("检测到修改了设备，刷新版本号...\n");
            versionSelect.Items = [];

            if (modelSelect.SelectedIndex != -1)
            {
                if (queryData != null && modelSelect.SelectedValue != null)
                {
                    foreach (var version in queryData[modelSelect.SelectedValue.ToString()])
                    {
                        versionSelect.Items.Add(version.Key);
                        log($"添加版本: {version.Key}");
                    }
                    versionSelect.SelectedValue = versionSelect.Items[0];
                    versionSelect.SelectedIndex = 0;
                }
                else
                {
                    showMsg("Error", $"找不到型号所对应的版本号 : {modelSelect.SelectedValue}", TType.Error);
                }
            }
            else
            {
                log($"选择了错误的 Index: {modelSelect.SelectedIndex}");
            }
        }

        private async Task LoadModelList()
        {
            queryData = await helper.GetAvaliableDictFromOnline();

            // Clear model list
            modelSelect.Items = [];
            log("加载设备列表...");
            if (queryData != null)
            {
                foreach (var model in queryData)
                {
                    modelSelect.Items.Add(model.Key);
                }

                if (modelSelect.Items.Count > 0)
                {
                    log("获取到设备型号列表:");
                    foreach (var item in modelSelect.Items)
                    {
                        log($"{item.ToString()}");
                    }
                    log("加载成功！");
                    //Ensure_Flash_Prepared();
                }
                else
                {
                    log("加载失败！");
                }
            }
            else
            {
                log("加载失败！\n");
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
                progressValue += 0.2f; // 每次增加 10
                ProgressBar.Value = progressValue;
                if (FlashSteps.Current != 2)
                {
                    FlashSteps.Current += 1;
                }
            }
            else
            {
                progressTimer.Stop(); // 停止 Timer

                // 延迟 1 秒后恢复
                System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ =>
                {
                    // 回到主线程重置进度条
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
                new AntdUI.Modal.Config(this, "关于...", "这个程序的开发是早茶光第三次骗了affggh写的\n求求你了静下心来好好写一个吧\n别老忽悠affggh了")
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
                            log("检测到设备！");
                            deviceBadge.Text = "已连接";
                            deviceBadge.State = AntdUI.TState.Success;
                            connectState = true;
                            // Set com port
                            var pattern = @"\bCOM\d+\b";
                            Match match = Regex.Match(deviceName, pattern);
                            if (match.Success)
                            {
                                comPort = match.Value;
                                log($"端口成功设置为: [{comPort}]");
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
            deviceBadge.Text = "未连接";
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
                    log("看起来你已经准备好了！");
                    FlashSteps.Current = 2;
                    flashButton.Enabled = true;
                    return;
                } else
                {
                    showMsg("Error:", "指定路径中 :\n" + dpath + "\n不存在rawprogram0.xml\n你确定这是一个线刷包目录？", TType.Error);
                }
            } else
            {
                showMsg("Error:", "设备未连接", TType.Error);
            }
            flashButton.Enabled = false;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            log("正在刷新设备状态...\n");
            DetectQualcommDeviceConnected();
            Ensure_Flash_Prepared();
            log("刷新完成！\n");

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
            //log($"下载进度{e.ProgressPercentage}");
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

            // 重置按钮状态或其他UI逻辑
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
                        showMsg("Warning:", "这个Rom已经下载了，不需要重复下载！", TType.Warn);
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
                showMsg("Error:", "请先将型号与版本进行选择，求求你了！", TType.Error);
            }

        }

        
    }

    
}

using AntdUI;

namespace XTCRecovery
{
    public partial class XorWindow : AntdUI.Window
    {
        public XorWindow()
        {
            InitializeComponent();

            BindEventHandler();

        }

        XTCRecovery_Helper helper = new XTCRecovery_Helper();
        private void showMsg(string title, string value, TType type)
        {
            AntdUI.Notification.open(new AntdUI.Notification.Config(this, title, value, type, TAlignFrom.BR) // Top right
            {
                Padding = new Size(24, 20),
                CloseIcon = true,
                //Link = new Notification.ConfigLink("Link to...", () =>
                //{
                //    AntdUI.Message.info(this, "Welcome!");
                //    return true;
                //})
            });
        }

        private void BindEventHandler()
        {
            uploadDragger.DragChanged += UploadDragger_DragChanged;
            uploadDragger.Click += UploadDragger_Click;
        }

        private async void UploadDragger_Click(object sender, EventArgs e)
        {
            // 创建一个文件对话框实例
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置文件对话框的属性
            openFileDialog.Filter = "All Files (*.*)|*.*"; // 过滤器，允许选择所有文件
            openFileDialog.Title = "Select a File"; // 对话框标题
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

                    if (!result)
                    {
                        showMsg("Error:", helper.GetLastErrorMessage(), TType.Error);
                    }
                    else
                    {
                        showMsg("Success:", $"Your file {path} has been success encrypted/decrypted!", TType.Success);
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
                if (!result)
                {
                    showMsg("Error:", helper.GetLastErrorMessage(), TType.Error);
                }
                else
                {
                    showMsg("Success:", $"Your file {path} has been success encrypted/decrypted!", TType.Success);
                }
            }
        }
    }
}

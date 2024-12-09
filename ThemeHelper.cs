using Microsoft.Win32;
using System.Drawing;

// Copied from AntdUI.Demos.Utils
namespace XTCRecovery
{
    public class ThemeHelper
    {
        /// <summary>
        /// 判断是否浅色
        /// </summary>
        /// <returns></returns>
        public static bool IsLightMode()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
            if (key != null)
            {
                int appsUseLightTheme = (int)key.GetValue("AppsUseLightTheme", -1);
                if (appsUseLightTheme == 1)
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置明暗颜色
        /// </summary>
        /// <param name="window"></param>
        /// <param name="isLight"></param>
        public static void SetColorMode(AntdUI.Window window, bool isLight)
        {
            if (!isLight)
            {
                AntdUI.Config.IsDark = true;// 设置为深色模式
                window.BackColor = Color.FromArgb(31, 31, 31);
                window.ForeColor = Color.White;
            }
            else
            {
                AntdUI.Config.IsLight = true;
                window.BackColor = Color.White;
                window.ForeColor = Color.Black;
            }
        }
    }
}

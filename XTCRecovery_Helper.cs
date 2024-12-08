using Newtonsoft.Json;
using System.Net.Http;

namespace XTCRecovery
{
    // If some return null pointer, call GetLastErrorMessage to show error message in ui
    internal class XTCRecovery_Helper
    {
        private static readonly HttpClient client = new HttpClient();

        private const string jsonQueryUrl = "https://cn-nb1.rains3.com/xtceasyrootplus/superrecovery.json";
        private string lastErrorMessage = "";

        private string prebuiltDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "prebuilt");
        private string romDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rom");

        internal XTCRecovery_Helper()
        {
            if (!Directory.Exists(romDir))
            {
                Directory.CreateDirectory(romDir);
            }
        }

        struct Xtc_xor_hdr
        {
            public int encrypt;
            public int index;
            public int count;
            public int unknow4;

            public Xtc_xor_hdr()
            {
                encrypt = 1;
                index = 41;
                count = 41;
                unknow4 = 0;
            }
        }

        private static readonly byte[] key_table = {
            0xF5, 0x89, 0x28, 0x66, 0x68, 0x3F, 0xB9, 0xED,
            0x7D, 0xDC, 0xCA, 0x7A, 0x37, 0x7B, 0xE0, 0xF9,
            0x04, 0xF8, 0xD2, 0xAE, 0x17, 0xCF, 0xC2, 0x61,
            0x08, 0x5D, 0x13, 0x90, 0x37, 0x0B, 0xC5, 0x3D,
            0x0F, 0xAA, 0xD6, 0x37, 0x28, 0x87, 0x92, 0x06,
            0xB4, 0x2E, 0x6B, 0xAF, 0x11, 0x36, 0x45, 0x5F,
            0x76, 0x2F, 0x19, 0x31, 0xC3, 0xDF, 0x72, 0xE8,
            0x90, 0xF6, 0x4F, 0x06, 0x6B, 0xE7, 0x28, 0x0F,
            0xD0, 0x90, 0x7C, 0xA1, 0x73, 0x2E, 0x39, 0x83,
            0x5F, 0x85, 0xAB, 0x07, 0x87, 0x84, 0x13, 0x29,
            0x58, 0x26, 0x7B, 0x7A, 0xF6, 0x2F, 0xD9, 0x93,
            0x43, 0x87, 0x37, 0xB0, 0x54, 0x15, 0xA5, 0x9D,
            0xC9, 0x0B, 0x66, 0xA1, 0xDF, 0x2D, 0x27, 0x26,
            0x12, 0x16, 0x65, 0xC3, 0x04, 0x98, 0x2C, 0xD4,
            0xDF, 0x64, 0x22, 0x5E, 0xE8, 0xE7, 0xE2, 0x56,
            0xE3, 0x9E, 0xE5, 0x75, 0x3D, 0x9A, 0x82, 0x74,
            0xD1, 0x54, 0xDE, 0xFA, 0xCA, 0x44, 0x1D, 0x9E,
            0xA2, 0xA7, 0xB1, 0xC1, 0xC6, 0xD7, 0x16, 0xF9,
            0x4E, 0xA7, 0x03, 0x4B, 0x03, 0x40, 0x9F, 0x0F,
            0x13, 0x98, 0xC9, 0x41, 0x0F, 0xD0, 0x24, 0x18,
            0xD0, 0x90, 0x7C, 0xA1, 0x73, 0x2E, 0x39, 0x83,
            0x5F, 0x85, 0xAB, 0x07, 0x87, 0x84, 0x13, 0x29,
            0x58, 0x26, 0x7B, 0x7A, 0xF6, 0x2F, 0xD9, 0x93,
            0x43, 0x87, 0x37, 0xB0, 0x54, 0x15, 0xA5, 0x9D,
            0x14, 0x07, 0xA8, 0x76, 0x02, 0xD4, 0xF7, 0x1D,
            0x22, 0xB4, 0xC9, 0x17, 0x21, 0xF8, 0x3E, 0x39,
            0x14, 0xE1, 0x3D, 0x13, 0x5D, 0x81, 0x66, 0x61,
            0xA6, 0x9F, 0x88, 0x45, 0xB9, 0x00, 0x53, 0xB3,
            0x9B, 0x0A, 0x79, 0x13, 0x60, 0x82, 0x93, 0x0A,
            0x91, 0x27, 0x8F, 0xC7, 0x16, 0xFC, 0xE2, 0x9E,
            0x42, 0x3D, 0xED, 0xF6, 0x0A, 0x87, 0xF9, 0x10,
            0xC0, 0x7F, 0x73, 0x3D, 0x59, 0x64, 0x6A, 0x93,
        };

        public bool Xor_file(string filepath)
        {
            // init config
            Xtc_xor_hdr hdr = new Xtc_xor_hdr();

            FileInfo fileInfo = new FileInfo(filepath);
            if (!fileInfo.Exists)
            {
                lastErrorMessage = "File does not exist";
                return false;
            }

            var filesize = fileInfo.Length;

            try
            {
                using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.ReadWrite))
                {
                    byte[] buffer = new byte[filesize > 0x8000 ? 32 : filesize];

                    fileStream.Read(buffer, 0, buffer.Length);

                    int cur = 0;
                    int key_index = 0;
                    do
                    {
                        key_index = (cur + hdr.unknow4) % 64;
                        buffer[cur] ^= key_table[key_index];
                        buffer[cur] ^= key_table[hdr.index];

                        if (key_index == 0)
                        {
                            ++hdr.count;
                            ++hdr.index;
                            if (hdr.count >= 256)
                            {
                                hdr.count = 0;
                                hdr.index = 0;
                            }
                        }
                        cur++;
                    } while (cur < buffer.Length);

                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Write(buffer, 0, buffer.Length);
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                lastErrorMessage = ex.Message;
                return false;
            }

            return true;
        }


        public string getRomDir()
        {
            return romDir;
        }

        public string getPrebuiltDir()
        {
            return prebuiltDir;
        }
        public async Task<Dictionary<string, Dictionary<string, string>>?> GetAvaliableDictFromOnline()
        {
            try
            {
                // 查询网络数据
                string json = await client.GetStringAsync(jsonQueryUrl);

                // 解析 JSON 数据为 Dictionary<string, string>
                var dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);

                return dict;  // 返回解析后的字典
            }
            catch (Exception e)
            {
                lastErrorMessage = e.Message;  // 错误信息存储
                return null;  // 出现异常时返回 null
            }
        }

        public string GetLastErrorMessage() => lastErrorMessage;  // 获取错误消息的方法
    }
}

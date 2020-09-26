using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class From1 : Form
    {
        private Config config;
        private CallInterface CallCapacity;

        public From1()
        {
            InitializeComponent();

            this.Initial();

            this.InitButtonEnabled();
        }

        /// <summary>
        /// 程序初始化执行方法
        /// </summary>
        private void Initial()
        {
            // 生成配置
            config = new Config();

            // 生成调用方法
            // 判断系统版本
            Version currentVersion = Environment.OSVersion.Version;
            Version compareToVersion = new Version("6.2");

            if (currentVersion.CompareTo(compareToVersion) >= 0)
            {
                CallCapacity = new CallCapacity(config);
            }
            else
            {
                CallCapacity = new Windows7Call(config);
            }   
        }

        /// <summary>
        /// 按钮可点击状态
        /// </summary>
        private void InitButtonEnabled()
        {
            this.button1.Enabled = !config.DirectoryExist();

            if (config.DirectoryExist())
            {
                this.button2.Enabled = !this.button2.Enabled;
                this.button3.Enabled = !this.button2.Enabled;
            }
            else
            {
                this.button2.Enabled = false;
                this.button3.Enabled = false;
            }
        }

        /// <summary>
        /// 项目初始化按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            // 移动项目文件夹
            // 判断指定路径是否存在文件夹
            if (!config.DirectoryExist())
            {
                //如果不存在就复制文件夹路径到这个目录去
                if (Directory.Exists("./localhost") == false)
                {
                    MessageBox.Show("请将项目文件夹放置软件同级目录");
                    return;
                }

                // 执行目录转移
                CopyDir("./localhost", config.ServePath);

                // 按钮状态变更
                InitButtonEnabled();

                // 复制当前可执行文件到所有用户的桌面
                // CopySelfToDesktop();

                MessageBox.Show("项目初始化完成");
            }
            else
            {
                MessageBox.Show("项目已初始化请勿重复点击!");
            }
        }

        /// <summary>
        /// 复制本执行文件到所有用户桌面
        /// </summary>
        private void CopySelfToDesktop()
        {
            string[] dirs = Directory.GetDirectories("C:/Users");

            // Contains
            string[] extDir = { "All Users", "Default", "Default User", "Public" };

            Process processes = Process.GetCurrentProcess();
            string name = processes.ProcessName;

            foreach (var dir in dirs)
            {
                if (!dir.Contains(extDir[0]) && !dir.Contains(extDir[1]) && !dir.Contains(extDir[2]) && !dir.Contains(extDir[3]))
                {
                    // 判断文件
                    if (File.Exists(dir + "/Desktop/" + name + ".exe")) {
                        // 删除文件
                        File.Delete(dir + "/Desktop/" + name + ".exe");
                    }
                    // 写入文件
                    File.Copy("./" + name + ".exe", dir + "/Desktop/" + name + ".exe");
                }
            }
        }

        /// <summary>
        /// 目录文件转移
        /// </summary>
        /// <param name="fromDir"></param>
        /// <param name="toDir"></param>
        public static void CopyDir(string fromDir, string toDir)
        {
            // 判断目标路径是否存在
            if (Directory.Exists(toDir))
            {
                // 删除旧的文件夹
                Directory.Delete(toDir, true);
            }

            // 创建目录和子目录
            Directory.CreateDirectory(toDir);

            // 获取所有的文件名称
            string[] Files = Directory.GetFiles(fromDir);

            // 复制所有的文件
            foreach (string FilePath in Files)
            {
                // 获取文件名称
                string FileName = Path.GetFileName(FilePath);

                // 取得目标文件路径
                string NewPath = new StringBuilder(toDir).Append("\\").Append(FileName).ToString();

                // 执行文件拷贝
                File.Copy(FilePath, NewPath, true);//复制
            }


            // 获取文件夹名称
            string[] Directorys = Directory.GetDirectories(fromDir);

            // 循环迭代
            foreach (string Directory in Directorys)
            {
                CopyDir(
                    Directory,
                    new StringBuilder(toDir).Append("\\").Append(Path.GetFileName(Directory)).ToString()
                    );
            }
        }

        /// <summary>
        /// 这里执行启动程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// Set ws = CreateObject("Wscript.Shell")
        /// ws.run "cmd /c C:\Users\Public\localhost\nginx.bat",vbhide
        /// ws.run "cmd /c C:\Users\Public\localhost\php.bat", vbhide
        /// ws.run "cmd /c C:\Users\Public\localhost\mysql.bat", vbhide
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // 调用停止操作
                CallCapacity.StopProcess();

                // 调用启动命令
                CallCapacity.MySQL();
                // CallCapacity.Redis();
                CallCapacity.PHP();
                CallCapacity.Nginx();

                // 程序运行修改按钮状态
                InitButtonEnabled();

                OpenWeb();
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序运行错误" + ex.Message);
            }
        }

        /// <summary>
        /// 打开网页
        /// </summary>
        private void OpenWeb()
        {
            try
            {
                Process.Start("http://localhost/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        /// <summary>
        /// 结束程序运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // 程序结束执行
                CallCapacity.StopProcess();

                // 程序运行修改按钮状态
                InitButtonEnabled();
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序运行错误" + ex.Message);
            }
        }

        /// <summary>
        /// 项目清理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // 判断原路径是否存在
                if (config.DirectoryExist())
                {
                    // 调用停止操作
                    CallCapacity.StopProcess();

                    // 删除目录
                    Directory.Delete(config.ServePath, true);

                    // 按钮状态变更
                    InitButtonEnabled();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("程序运行错误" + ex.Message);
            }
        }
    }
}

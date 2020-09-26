using System.Diagnostics;

namespace WindowsFormsApp1
{
    class Windows7Call : CallInterface
    {
        private Config config;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="config"></param>
        public Windows7Call(Config config)
        {
            this.config = config;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~Windows7Call()
        {
            StopProcess();
        }

        /// <summary>
        /// 获取cmd
        /// </summary>
        /// <returns></returns>
        private void StartProcess(string BatName)
        {
            //创建一个进程
            Process proc = new Process();

            // 程序名称
            proc.StartInfo.FileName = string.Format("{0}/{1}", config.ChildPath("win"), BatName);

            // 参数
            proc.StartInfo.Arguments = string.Format("10");
            
            // 启动时是否显示窗口
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            proc.Start();
        }

        /// <summary>
        /// 启动MySQL
        /// </summary>
        public void MySQL()
        {
            StartProcess(config.Is64System() ? "mysql.bat" : "mysql32.bat");
        }

        /// <summary>
        /// 启动nginx
        /// </summary>
        public void Nginx()
        {
            StartProcess("nginx.bat");
        }

        /// <summary>
        /// 启动PHP
        /// </summary>
        public void PHP()
        {
            StartProcess(config.Is64System() ? "php.bat" : "php32.bat");
        }

        /// <summary>
        /// 启动redis
        /// </summary>
        public void Redis()
        {
            StartProcess("nginx.bat");
        }

        /// <summary>
        /// 停止程序
        /// </summary>
        public void StopProcess()
        {
            Process[] pros = Process.GetProcesses();

            foreach (Process process in pros)
            {
                if (
                    process.ProcessName.ToLower().Contains("nginx") ||
                    process.ProcessName.ToLower().Contains("php-cgi") ||
                    process.ProcessName.ToLower().Contains("mysqld") ||
                    process.ProcessName.ToLower().Contains("redis")
                    )
                {
                    process.Kill();
                }
            }
        }
    }
}

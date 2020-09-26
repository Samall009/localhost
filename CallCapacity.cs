using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsFormsApp1
{
    class CallCapacity : CallInterface
    {
        /// <summary>
        /// 系统整体配置参数
        /// </summary>
        public Config Config { get; private set; }

        /// <summary>
        /// 引用dll
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpszOp"></param>
        /// <param name="lpszFile"></param>
        /// <param name="lpszParams"></param>
        /// <param name="lpszDir"></param>
        /// <param name="FsShowCmd"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern int ShellExecute(
            IntPtr hwnd,
            StringBuilder lpszOp,
            StringBuilder lpszFile,
            StringBuilder lpszParams,
            StringBuilder lpszDir,
            int FsShowCmd
        );

        /// <summary>
        /// 构造方法
        /// </summary>
        public CallCapacity(Config config)
        {
            this.Config = config;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~CallCapacity()
        {
            StopProcess();
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

        /// <summary>
        /// 调用PHP启动方法
        /// </summary>
        public void PHP()
        {
            // 调用启动方法
            ShellExecute(
                IntPtr.Zero,
                new StringBuilder("Open"),
                new StringBuilder("php-cgi.exe"),
                new StringBuilder("-b 127.0.0.1:9049"),
                new StringBuilder(@Config.ChildPath(Config.Is64System() ? "php" : "php32")),
                0);
        }

        /// <summary>
        /// 调用Nginx启动方法
        /// </summary>
        public void Nginx()
        {
            ShellExecute(
                IntPtr.Zero, 
                new StringBuilder("Open"), 
                new StringBuilder("nginx.exe"), 
                new StringBuilder(""), 
                new StringBuilder(@Config.ChildPath("nginx")),
                0);
        }

        /// <summary>
        /// 启动MySQL
        /// </summary>
        public void MySQL()
        {
            ShellExecute(
                IntPtr.Zero,
                new StringBuilder("Open"),
                new StringBuilder("mysqld.exe"),
                new StringBuilder(""),
                new StringBuilder(@Config.ChildPath(Config.Is64System() ? "mysql/bin" : "mysql32/bin")),
                0);
        }

        /// <summary>
        /// 启动Redis
        /// </summary>
        public void Redis()
        {
            ShellExecute(
                IntPtr.Zero,
                new StringBuilder("Open"),
                new StringBuilder("redis-server.exe"),
                new StringBuilder("redis.conf"),
                new StringBuilder(@Config.ChildPath("redis")),
                0);
        }
    }
}

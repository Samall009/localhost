using System;
using System.IO;
using System.Text;

namespace WindowsFormsApp1
{
    public partial class Config
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public Config()
        {
            ServePath = "C:\\localhost";
        }

        /// <summary>
        /// 服务路径
        /// </summary>
        public string ServePath { get; private set; }

        /// <summary>
        /// 获取类目录子路径
        /// </summary>
        /// <param name="name">子目录名称</param>
        /// <returns></returns>
        public string ChildPath(string name)
        {
            return new StringBuilder(ServePath).Append("\\").Append(name).ToString();
        }

        /// <summary>
        /// 路径是否存在
        /// </summary>
        /// <returns></returns>
        public Boolean DirectoryExist()
        {
            return Directory.Exists(ServePath);
        }

        /// <summary>
        /// 是否是64位系统
        /// </summary>
        /// <returns></returns>
        public Boolean Is64System()
        {
            return Environment.Is64BitOperatingSystem;
        }
    }
}

namespace WindowsFormsApp1
{
    interface CallInterface
    {
        /// <summary>
        /// 停止程序
        /// </summary>
        void StopProcess();

        /// <summary>
        /// 调用PHP启动方法
        /// </summary>
        void PHP();

        /// <summary>
        /// 调用Nginx启动方法
        /// </summary>
        void Nginx();

        /// <summary>
        /// 启动MySQL
        /// </summary>
        void MySQL();

        /// <summary>
        /// 启动Redis
        /// </summary>
        void Redis();
    }
}

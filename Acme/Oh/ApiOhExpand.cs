namespace Acme
{
    /// <summary>
    /// Oh拓展类
    /// </summary>
    public static class ApiOhExpand
    {
        /// <summary>
        /// 获取成功时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static ApiOh<TEntity> ApiYes<TEntity>(
            this TEntity data,
            string msg = CV.Yes)
        {
            return new ApiOh<TEntity>(CV.YesCode, msg, data);
        }

        /// <summary>
        /// 获取成功时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static ApiOh<TEntity> ApiYes<TEntity>(
            this TEntity data)
        {
            return new ApiOh<TEntity>(CV.YesCode, CV.Yes, data);
        }

        /// <summary>
        /// 获取成功时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="code">状态码</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static ApiOh<TEntity> ApiYes<TEntity>(
            this TEntity data,
            int code = CV.YesCode,
            string msg = CV.Yes)
        {
            return new ApiOh<TEntity>(code, msg, data);
        }

        /// <summary>
        /// 获取失败时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static ApiOh<TEntity> ApiNo<TEntity>(
            this TEntity data,
            string msg = CV.ErrorServer)
        {
            return new ApiOh<TEntity>(CV.NoCode, msg, data);
        }

        /// <summary>
        /// 获取失败时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static ApiOh<TEntity> ApiNo<TEntity>(
            this TEntity data)
        {
            return new ApiOh<TEntity>(CV.NoCode, CV.ErrorServer, data);
        }

        /// <summary>
        /// 获取失败时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="code">状态码</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static ApiOh<TEntity> ApiNo<TEntity>(
            this TEntity data,
            int code = CV.NoCode,
            string msg = CV.ErrorServer)
        {
            return new ApiOh<TEntity>(code, msg, data);
        }

        /// <summary>
        /// 自定义状态码
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="code">状态码</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static ApiOh<TEntity> ApiCustom<TEntity>(
            this TEntity data,
            int code,
            string msg)
        {
            return new ApiOh<TEntity>(code, msg, data);
        }
    }
}

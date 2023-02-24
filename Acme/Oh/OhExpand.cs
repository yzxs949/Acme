namespace Acme
{
    /// <summary>
    /// Oh拓展类
    /// </summary>
    public static class OhExpand
    {
        /// <summary>
        /// 获取成功时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static Oh Yes(
            this object data,
            string msg = CV.Yes)
        {
            return new Oh(CV.YesCode, msg, data);
        }

        /// <summary>
        /// 获取成功时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static Oh Yes(
            this object data)
        {
            return new Oh(CV.YesCode, CV.Yes, data);
        }

        /// <summary>
        /// 获取成功时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="code">状态码</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static Oh Yes(
            this object data,
            int code = CV.YesCode,
            string msg = CV.Yes)
        {
            return new Oh(code, msg, data);
        }

        /// <summary>
        /// 获取失败时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static Oh No(
            this object data,
            string msg = CV.ErrorServer)
        {
            return new Oh(CV.NoCode, msg, data);
        }

        /// <summary>
        /// 获取失败时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static Oh No(
            this object data)
        {
            return new Oh(CV.NoCode, CV.ErrorServer, data);
        }

        /// <summary>
        /// 获取失败时的Oh值
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="code">状态码</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static Oh No(
            this object data,
            int code = CV.NoCode,
            string msg = CV.ErrorServer)
        {
            return new Oh(code, msg, data);
        }

        /// <summary>
        /// 自定义状态码
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="code">状态码</param>
        /// <param name="msg">描述</param>
        /// <returns></returns>
        public static Oh Custom(
            this object data,
            int code,
            string msg)
        {
            return new Oh(code, msg, data);
        }
    }
}

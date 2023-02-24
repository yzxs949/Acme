namespace Acme
{
    /// <summary>
    /// 
    /// </summary>
    public class Oh
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="msg">描述</param>
        /// <param name="data">数据</param>
        public Oh(int code = CV.YesCode,
                  string msg = CV.BlankCharacter,
                  object data = null)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// 实例化Oh
        /// </summary>
        /// <returns></returns>
        public static Oh NewOh()
        {
            return new Oh();
        }

        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }
    }
}

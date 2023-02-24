namespace Acme
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class ApiOh<TEntity>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="msg">描述</param>
        /// <param name="data">数据</param>
        public ApiOh(int code = CV.YesCode,
                  string msg = CV.BlankCharacter,
                  TEntity data = default)
        {
            this.Code = code;
            this.Msg = msg;
            this.Data = data;
        }

        /// <summary>
        /// 实例化Oh
        /// </summary>
        /// <returns></returns>
        public static ApiOh<TEntity> NewOh()
        {
            return new ApiOh<TEntity>();
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
        public TEntity Data { get; set; }
    }
}

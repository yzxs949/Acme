using Microsoft.Extensions.DependencyInjection;

namespace Acme.Dapper
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static class DapperHelperServer
    {
        /// <summary>
        /// 注册FastEnterprise.Dapper的方法
        /// </summary>
        /// <param name="_services">IServiceCollection</param>
        /// <param name="_connectionString">数据库连接字符</param>
        public static void AddDapperServer(
            this IServiceCollection _services,
            string _connectionString)
        {
            IDapperHelper helper = new DapperHelper(_connectionString);
            _services.AddTransient(IDapperHelper => helper);
            _services.AddScoped(typeof(ISplicing<>), typeof(Splicing<>));
            _services.AddScoped(typeof(IDapperEntity<>), typeof(DapperEntity<>));
        }
    }
}
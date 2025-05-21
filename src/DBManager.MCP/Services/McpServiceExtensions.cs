using DBManager.MCP.Client;
using DBManager.MCP.Protocol;
using Microsoft.Extensions.DependencyInjection;

namespace DBManager.MCP.Services
{
    /// <summary>
    /// MCP服务扩展
    /// </summary>
    public static class McpServiceExtensions
    {
        /// <summary>
        /// 添加MCP服务到依赖注入容器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="mcpServerUrl">MCP服务器URL</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddMcpServices(this IServiceCollection services, string mcpServerUrl)
        {
            // 注册MCP客户端
            services.AddSingleton<IMcpClient>(provider => new McpClient(mcpServerUrl));
            
            // 注册MCP协议处理器
            services.AddSingleton<IMcpProtocolHandler, McpProtocolHandler>();
            
            // 注册MCP连接管理器
            services.AddSingleton<IMcpConnectionManager, McpConnectionManager>();
            
            return services;
        }
    }
}

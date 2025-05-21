using DBManager.MCP.Models;

namespace DBManager.MCP.Protocol
{
    /// <summary>
    /// MCP协议处理器接口
    /// </summary>
    public interface IMcpProtocolHandler
    {
        /// <summary>
        /// 处理MCP请求
        /// </summary>
        /// <param name="request">MCP请求</param>
        /// <returns>MCP响应</returns>
        Task<McpResponse> HandleRequestAsync(McpRequest request);
        
        /// <summary>
        /// 获取可用工具列表
        /// </summary>
        /// <returns>工具列表</returns>
        Task<IEnumerable<McpTool>> GetAvailableToolsAsync();
    }
    
    /// <summary>
    /// MCP工具描述
    /// </summary>
    public class McpTool
    {
        /// <summary>
        /// 工具名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// 工具描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// 工具支持的操作
        /// </summary>
        public IEnumerable<string> SupportedActions { get; set; } = new List<string>();
    }
}

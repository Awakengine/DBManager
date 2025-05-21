using System;
using DBManager.MCP.Client;
using DBManager.MCP.Models;
using DBManager.MCP.Protocol;

namespace DBManager.MCP.Services
{
    /// <summary>
    /// MCP连接管理器接口
    /// </summary>
    public interface IMcpConnectionManager
    {
        /// <summary>
        /// 创建新连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>连接ID</returns>
        Task<string> CreateConnectionAsync(string connectionString);
        
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>是否成功</returns>
        Task<bool> CloseConnectionAsync(string connectionId);
        
        /// <summary>
        /// 获取连接状态
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>连接状态</returns>
        Task<ConnectionStatus> GetConnectionStatusAsync(string connectionId);
    }
    
    /// <summary>
    /// 连接状态
    /// </summary>
    public enum ConnectionStatus
    {
        /// <summary>
        /// 已连接
        /// </summary>
        Connected,
        
        /// <summary>
        /// 已断开
        /// </summary>
        Disconnected,
        
        /// <summary>
        /// 连接中
        /// </summary>
        Connecting,
        
        /// <summary>
        /// 未知
        /// </summary>
        Unknown
    }
    
    /// <summary>
    /// MCP连接管理器实现
    /// </summary>
    public class McpConnectionManager : IMcpConnectionManager
    {
        private readonly IMcpClient _mcpClient;
        private readonly Dictionary<string, ConnectionStatus> _connections;
        
        /// <summary>
        /// 初始化MCP连接管理器
        /// </summary>
        /// <param name="mcpClient">MCP客户端</param>
        public McpConnectionManager(IMcpClient mcpClient)
        {
            _mcpClient = mcpClient;
            _connections = new Dictionary<string, ConnectionStatus>();
        }
        
        /// <summary>
        /// 创建新连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>连接ID</returns>
        public async Task<string> CreateConnectionAsync(string connectionString)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "create_connection" },
                    { "connection_string", connectionString }
                }
            };
            
            var response = await _mcpClient.SendRequestAsync(request);
            
            if (response.Status == "success" && response.Result is Dictionary<string, object> result && 
                result.TryGetValue("connection_id", out var connectionId))
            {
                string connId = connectionId.ToString() ?? Guid.NewGuid().ToString();
                _connections[connId] = ConnectionStatus.Connected;
                return connId;
            }
            
            throw new Exception($"Failed to create connection: {response.Error?.Message ?? "Unknown error"}");
        }
        
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>是否成功</returns>
        public async Task<bool> CloseConnectionAsync(string connectionId)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "close_connection" },
                    { "connection_id", connectionId }
                }
            };
            
            var response = await _mcpClient.SendRequestAsync(request);
            
            if (response.Status == "success")
            {
                _connections.Remove(connectionId);
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// 获取连接状态
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>连接状态</returns>
        public Task<ConnectionStatus> GetConnectionStatusAsync(string connectionId)
        {
            if (_connections.TryGetValue(connectionId, out var status))
            {
                return Task.FromResult(status);
            }
            
            return Task.FromResult(ConnectionStatus.Unknown);
        }
    }
}

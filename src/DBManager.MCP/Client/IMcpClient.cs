using System.Text.Json;
using DBManager.MCP.Models;

namespace DBManager.MCP.Client
{
    /// <summary>
    /// MCP客户端接口
    /// </summary>
    public interface IMcpClient
    {
        /// <summary>
        /// 发送MCP请求并获取响应
        /// </summary>
        /// <param name="request">MCP请求</param>
        /// <returns>MCP响应</returns>
        Task<McpResponse> SendRequestAsync(McpRequest request);
        
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="query">SQL查询语句</param>
        /// <returns>查询结果</returns>
        Task<McpResponse> ExecuteQueryAsync(string connectionId, string query);
        
        /// <summary>
        /// 获取数据库架构信息
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>数据库架构信息</returns>
        Task<McpResponse> GetSchemaAsync(string connectionId);
        
        /// <summary>
        /// 获取表列表
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>表列表</returns>
        Task<McpResponse> ListTablesAsync(string connectionId);
        
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>事务ID</returns>
        Task<McpResponse> BeginTransactionAsync(string connectionId);
        
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="transactionId">事务ID</param>
        /// <returns>操作结果</returns>
        Task<McpResponse> CommitTransactionAsync(string connectionId, string transactionId);
        
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="transactionId">事务ID</param>
        /// <returns>操作结果</returns>
        Task<McpResponse> RollbackTransactionAsync(string connectionId, string transactionId);
    }
}

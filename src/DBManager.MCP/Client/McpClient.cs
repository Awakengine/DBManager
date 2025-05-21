using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DBManager.MCP.Models;

namespace DBManager.MCP.Client
{
    /// <summary>
    /// MCP客户端实现
    /// </summary>
    public class McpClient : IMcpClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// 初始化MCP客户端
        /// </summary>
        /// <param name="baseUrl">MCP服务器基础URL</param>
        public McpClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        /// <summary>
        /// 发送MCP请求并获取响应
        /// </summary>
        /// <param name="request">MCP请求</param>
        /// <returns>MCP响应</returns>
        public async Task<McpResponse> SendRequestAsync(McpRequest request)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(request, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_baseUrl}/mcp", content);
                response.EnsureSuccessStatusCode();

                var mcpResponse = await response.Content.ReadFromJsonAsync<McpResponse>(_jsonOptions);
                return mcpResponse ?? new McpResponse
                {
                    RequestId = request.RequestId,
                    Status = "error",
                    Error = new McpError
                    {
                        Code = "response_parse_error",
                        Message = "Failed to parse MCP response"
                    }
                };
            }
            catch (Exception ex)
            {
                return new McpResponse
                {
                    RequestId = request.RequestId,
                    Status = "error",
                    Error = new McpError
                    {
                        Code = "request_failed",
                        Message = ex.Message
                    }
                };
            }
        }

        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="query">SQL查询语句</param>
        /// <returns>查询结果</returns>
        public async Task<McpResponse> ExecuteQueryAsync(string connectionId, string query)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "execute_query" },
                    { "connection_id", connectionId },
                    { "query", query }
                }
            };

            return await SendRequestAsync(request);
        }

        /// <summary>
        /// 获取数据库架构信息
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>数据库架构信息</returns>
        public async Task<McpResponse> GetSchemaAsync(string connectionId)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "get_schema" },
                    { "connection_id", connectionId }
                }
            };

            return await SendRequestAsync(request);
        }

        /// <summary>
        /// 获取表列表
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>表列表</returns>
        public async Task<McpResponse> ListTablesAsync(string connectionId)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "list_tables" },
                    { "connection_id", connectionId }
                }
            };

            return await SendRequestAsync(request);
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <returns>事务ID</returns>
        public async Task<McpResponse> BeginTransactionAsync(string connectionId)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "begin_transaction" },
                    { "connection_id", connectionId }
                }
            };

            return await SendRequestAsync(request);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="transactionId">事务ID</param>
        /// <returns>操作结果</returns>
        public async Task<McpResponse> CommitTransactionAsync(string connectionId, string transactionId)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "commit_transaction" },
                    { "connection_id", connectionId },
                    { "transaction_id", transactionId }
                }
            };

            return await SendRequestAsync(request);
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="connectionId">连接ID</param>
        /// <param name="transactionId">事务ID</param>
        /// <returns>操作结果</returns>
        public async Task<McpResponse> RollbackTransactionAsync(string connectionId, string transactionId)
        {
            var request = new McpRequest
            {
                Tool = "database",
                Parameters = new Dictionary<string, object>
                {
                    { "action", "rollback_transaction" },
                    { "connection_id", connectionId },
                    { "transaction_id", transactionId }
                }
            };

            return await SendRequestAsync(request);
        }
    }
}

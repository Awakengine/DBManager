using System.Text.Json.Serialization;

namespace DBManager.MCP.Models
{
    /// <summary>
    /// 表示MCP协议的响应模型
    /// </summary>
    public class McpResponse
    {
        /// <summary>
        /// 响应对应的请求ID
        /// </summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; } = string.Empty;

        /// <summary>
        /// 响应状态
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; } = "success";

        /// <summary>
        /// 响应结果
        /// </summary>
        [JsonPropertyName("result")]
        public object? Result { get; set; }

        /// <summary>
        /// 错误信息（如果有）
        /// </summary>
        [JsonPropertyName("error")]
        public McpError? Error { get; set; }
    }

    /// <summary>
    /// MCP错误信息
    /// </summary>
    public class McpError
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 错误消息
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}

using System.Text.Json.Serialization;

namespace DBManager.MCP.Models
{
    /// <summary>
    /// 表示MCP协议的请求模型
    /// </summary>
    public class McpRequest
    {
        /// <summary>
        /// 请求的唯一标识符
        /// </summary>
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 请求的工具名称
        /// </summary>
        [JsonPropertyName("tool")]
        public string Tool { get; set; } = string.Empty;

        /// <summary>
        /// 请求的参数
        /// </summary>
        [JsonPropertyName("parameters")]
        public Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
    }
}

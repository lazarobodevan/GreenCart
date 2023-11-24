using System.Text.Json;

namespace backend.Utils {
    public class ApiResponse {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status {  get; set; }
        public string TraceId {  get; set; }
        public JsonElement Data {  get; set; }
        public Dictionary<string, List<string>>? Errors {  get; set; }
    }
}

using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessengerWeb.Shared
{

    public class FaceApiTaskRequest
    {
        [JsonPropertyName("engine_id")]
        public string EngineId { get; set; }

        [JsonPropertyName("file_hash")]
        public string FileHash { get; set; }
    }

    public class FaceApiTaskResponse
    {
        [JsonPropertyName("task_id")]
        public string TaskId { get; set; }
    }

    public interface IFaceApiTaskResult { }

    public class LivenessTaskResult : IFaceApiTaskResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public LivenessResult Result { get; set; }
    }

    public class LivenessResult
    {
        [JsonPropertyName("score")]
        public double Score { get; set; }
    }

    public class CommonTaskResult : IFaceApiTaskResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("result")]
        public MatchResult Result { get; set; }
    }

    public class MatchResult
    {
        [JsonPropertyName("face_id")]
        public string FaceId { get; set; }
    }

    public class FaceApiTaskResultError : IFaceApiTaskResult
    {
        [JsonPropertyName("detail")]
        public List<Detail> Detail { get; set; }
    }

    public class Detail
    {
        [JsonPropertyName("loc")]
        public List<string> Loc { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}

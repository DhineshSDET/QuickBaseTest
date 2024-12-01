using Newtonsoft.Json;

namespace QuickBaseApiTest.Model.v1;

public class DeleteFieldResponse
{
    [JsonProperty("deletedFieldIds")] public List<int> DeletedFieldIds { get; set; }

    [JsonProperty("errors")] public List<string> Errors { get; set; }
}
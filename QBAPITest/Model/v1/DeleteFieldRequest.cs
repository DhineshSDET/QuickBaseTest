using Newtonsoft.Json;

namespace QuickBaseApiTest.Model.v1;

public class DeleteFieldRequest
{
    [JsonProperty("fieldIds")] public List<int> FieldIds { get; set; }
}
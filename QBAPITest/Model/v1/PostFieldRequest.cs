using Newtonsoft.Json;

namespace QuickBaseApiTest.Model.v1;

public class PostFieldRequest
{
    [JsonProperty("label")] public string Label { get; set; }

    [JsonProperty("fieldType")] public string FieldType { get; set; }

    [JsonProperty("noWrap")] public bool NoWrap { get; set; }

    [JsonProperty("bold")] public bool Bold { get; set; }

    [JsonProperty("appearsByDefault")] public bool AppearsByDefault { get; set; }

    [JsonProperty("findEnabled")] public bool FindEnabled { get; set; }

    [JsonProperty("fieldHelp")] public string FieldHelp { get; set; }

    [JsonProperty("addToForms")] public bool AddToForms { get; set; }

    [JsonProperty("properties")] public Properties properties { get; set; }

    [JsonProperty("permissions")] public List<Permission> Permissions { get; set; }


    public class Permission
    {
        [JsonProperty("role")] public string Role { get; set; }

        [JsonProperty("permissionType")] public string PermissionType { get; set; }

        [JsonProperty("roleId")] public int RoleId { get; set; }
    }

    public class Properties
    {
        [JsonProperty("maxLength")] public int MaxLength { get; set; }

        [JsonProperty("appendOnly")] public bool AppendOnly { get; set; }

        [JsonProperty("sortAsGiven")] public bool SortAsGiven { get; set; }
    }
}
using Newtonsoft.Json;

namespace QuickBaseApiTest.Model.v1;

public class PostFieldResponse
{
    [JsonProperty("id")] public int Id { get; set; }

    [JsonProperty("label")] public string Label { get; set; }

    [JsonProperty("fieldType")] public string FieldType { get; set; }

    [JsonProperty("mode")] public string Mode { get; set; }

    [JsonProperty("noWrap")] public bool NoWrap { get; set; }

    [JsonProperty("bold")] public bool Bold { get; set; }

    [JsonProperty("required")] public bool Required { get; set; }

    [JsonProperty("appearsByDefault")] public bool AppearsByDefault { get; set; }

    [JsonProperty("findEnabled")] public bool FindEnabled { get; set; }

    [JsonProperty("unique")] public bool Unique { get; set; }

    [JsonProperty("doesDataCopy")] public bool DoesDataCopy { get; set; }

    [JsonProperty("fieldHelp")] public string FieldHelp { get; set; }

    [JsonProperty("audited")] public bool Audited { get; set; }

    [JsonProperty("properties")] public Properties properties { get; set; }

    [JsonProperty("permissions")] public List<Permission> Permissions { get; set; }

    public class Permission
    {
        [JsonProperty("permissionType")] public string PermissionType { get; set; }

        [JsonProperty("role")] public string Role { get; set; }

        [JsonProperty("roleId")] public int RoleId { get; set; }
    }

    public class Properties
    {
        [JsonProperty("primaryKey")] public bool PrimaryKey { get; set; }

        [JsonProperty("foreignKey")] public bool ForeignKey { get; set; }

        [JsonProperty("numLines")] public int NumLines { get; set; }

        [JsonProperty("maxLength")] public int MaxLength { get; set; }

        [JsonProperty("appendOnly")] public bool AppendOnly { get; set; }

        [JsonProperty("allowHTML")] public bool AllowHTML { get; set; }

        [JsonProperty("allowMentions")] public bool AllowMentions { get; set; }

        [JsonProperty("sortAsGiven")] public bool SortAsGiven { get; set; }

        [JsonProperty("carryChoices")] public bool CarryChoices { get; set; }

        [JsonProperty("allowNewChoices")] public bool AllowNewChoices { get; set; }

        [JsonProperty("formula")] public string Formula { get; set; }

        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }
    }
}
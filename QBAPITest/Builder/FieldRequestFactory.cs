using QuickBaseApiTest.Model.v1;
using static QuickBaseApiTest.Model.v1.PostFieldRequest;

namespace QuickBaseApiTest.Builder;

public static class FieldRequestFactory
{
    public static PostFieldRequest CreateFieldRequest(string label)
    {
        return new PostFieldRequest
        {
            Label = label,
            FieldType = "text",
            NoWrap = false,
            Bold = false,
            AppearsByDefault = false,
            FindEnabled = false,
            FieldHelp = "field help",
            AddToForms = true,
            properties = new Properties
            {
                MaxLength = 0,
                AppendOnly = false,
                SortAsGiven = false
            },
            Permissions = new List<Permission>
            {
                new() { Role = "Viewer", PermissionType = "View", RoleId = 10 },
                new() { Role = "Participant", PermissionType = "None", RoleId = 11 },
                new() { Role = "Administrator", PermissionType = "Modify", RoleId = 12 }
            }
        };
    }
}
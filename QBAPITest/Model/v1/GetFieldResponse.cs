namespace QuickBaseApiTest.Model.v1;

public class GetFieldResponse
{
    public int id { get; set; }
    public string label { get; set; }
    public string fieldType { get; set; }
    public string mode { get; set; }
    public bool noWrap { get; set; }
    public bool bold { get; set; }
    public bool required { get; set; }
    public bool appearsByDefault { get; set; }
    public bool findEnabled { get; set; }
    public bool unique { get; set; }
    public bool doesDataCopy { get; set; }
    public string fieldHelp { get; set; }
    public bool audited { get; set; }
    public Properties properties { get; set; }
    public List<Permission> permissions { get; set; }

    public class Permission
    {
        public string permissionType { get; set; }
        public string role { get; set; }
        public int roleId { get; set; }
    }

    public class Properties
    {
        public bool primaryKey { get; set; }
        public bool foreignKey { get; set; }
        public int numLines { get; set; }
        public int maxLength { get; set; }
        public bool appendOnly { get; set; }
        public bool allowHTML { get; set; }
        public bool allowMentions { get; set; }
        public bool sortAsGiven { get; set; }
        public bool carryChoices { get; set; }
        public bool allowNewChoices { get; set; }
        public string formula { get; set; }
        public string defaultValue { get; set; }
    }
}
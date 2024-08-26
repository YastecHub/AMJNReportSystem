using System.ComponentModel;

namespace AMJNReportSystem.Domain.Enums
{
    public enum ResponseType
    {
        [Description("Integer")]
        Integer = 1,

        [Description("TextInput")]
        TextInput,

        [Description("Boolean")]
        Boolean,

        [Description("File")]
        File,

        Checkbox,
        Dropdown,
        Radio,
        Date,

    }
}

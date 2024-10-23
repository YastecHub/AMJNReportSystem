using System.ComponentModel;

namespace AMJNReportSystem.Domain.Enums
{
    public enum QuestionType
    {
        [Description("TextInput")]
        Text = 1,
        [Description("Dropdown")]
        Dropdown,
        [Description("File")]
        File,
        MultipleChoice,
        Checkbox,
        [Description("Radio")]
        Radio,
        [Description("Date")]
        Date
    }
}

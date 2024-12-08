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


    public enum Roles
    {
        [Description("Administrator")]
        Administrator = 1,

        [Description("Amir")]
        Amir = 12,

        [Description("Audio Visual Secretary")]
        AudioVisualSecretary = 33,

        [Description("Circuit Fin Sec")]
        CircuitFinSec = 7,

        [Description("Circuit General Sec")]
        CircuitGeneralSec = 27,

        [Description("Circuit Missionary")]
        CircuitMissionary = 10,

        [Description("Circuit President")]
        CircuitPresident = 5,

        [Description("Dhiyafat Secretary")]
        DhiyafatSecretary = 40,

        [Description("HQ Deputy Accountant")]
        HQDeputyAccountant = 9,

        [Description("HQ Accountant")]
        HQAccountant = 8,

        [Description("Isha’at Secretary")]
        IshaatSecretary = 41,

        [Description("Jaidad Secretary")]
        JaidadSecretary = 35,

        [Description("Jamaat Fin Sec")]
        JamaatFinSec = 6,

        [Description("Jamaat General Sec")]
        JamaatGeneralSec = 26,

        [Description("Jamaat Missionary")]
        JamaatMissionary = 11,

        [Description("Jamaat President")]
        JamaatPresident = 4,

        [Description("Member")]
        Member = 2,

        [Description("Naib Amir")]
        NaibAmir = 13,

        [Description("Naib Nazim Mal")]
        NaibNazimMal = 17,

        [Description("Naib Zaim Mal")]
        NaibZaimMal1 = 19,

        [Description("Naib Zaim Mal")]
        NaibZaimMal2 = 21,

        [Description("National General Sec")]
        NationalGeneralSec = 25,

        [Description("Nau Muba’een Secretary")]
        NauMubaeenSecretary = 42,

        [Description("Nazim A'la")]
        NazimAla = 16,

        [Description("Qaid Mal")]
        QaidMal = 15,

        [Description("Rishta Nata Secretary")]
        RishtaNataSecretary = 38,

        [Description("Sadr Majlis Ansarullah")]
        SadrMajlisAnsarullah = 14,

        [Description("Sanat-o-Tijarat Secretary")]
        SanatOTijaratSecretary = 47,

        [Description("Ta’leem Secretary")]
        TaleemSecretary = 31,

        [Description("Tabligh Secretary")]
        TablighSecretary = 30,

        [Description("Tahrik-i-Jadid Secretary")]
        TahrikIJadidSecretary = 44,

        [Description("TajneedOfficer-Circuit")]
        TajneedOfficerCircuit = 23,

        [Description("TajneedOfficer-Jamaat")]
        TajneedOfficerJamaat = 22,

        [Description("TajneedOfficer-National")]
        TajneedOfficerNational = 24,

        [Description("Tajnid Secretary")]
        TajnidSecretary = 28,

        [Description("Talimul Qur’an Secretary")]
        TalimulQuranSecretary = 39,

        [Description("Tarbiyya Secretary")]
        TarbiyyaSecretary = 32,

        [Description("Umur Aama Secretary")]
        UmurAamaSecretary = 34,

        [Description("Umur Kharijiiyah Secretary")]
        UmurKharijiiyahSecretary = 36,

        [Description("Waqar-e-Amal Secretary")]
        WaqarEAmalSecretary = 43,

        [Description("Waqf-i-Jadid Secretary")]
        WaqfiNauSecretary = 37,

        [Description("Wasiyya Secretary")]
        WasiyyaSecretary = 29,

        [Description("Zaim")]
        Zaim = 20,

        [Description("Zaim A'la")]
        ZaimAla = 18,

        [Description("Zira’at Secretary")]
        ZiraatSecretary = 46
    }

}

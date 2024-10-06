namespace AMJNReportSystem.Domain.Entities
{
    public class User
    {
        public string ChandaNo { get; set; }
        public string WasiyatNo { get; set; }
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string AuxillaryBodyName { get; set; }
        public string? MiddleName { get; set; }
        public string MaidenName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public int JamaatId { get; set; }
        public string? JamaatCode { get; set; }
        public string JamaatName { get; set; }
        public int CircuitId { get; set; }
        public string? CircuitCode { get; set; }
        public string CircuitName { get; set; }
        public string Sex { get; set; }
        public string MaritalStatus { get; set; }
        public string Address { get; set; }
        public string NextOfKinPhoneNo { get; set; }
        public bool RecordStatus { get; set; }
        public string MemberShipStatus { get; set; }
        public string Nationality { get; set; }
        public string? PhotoUrl { get; set; }
        public List<string>? Roles { get; set; }
    }
}
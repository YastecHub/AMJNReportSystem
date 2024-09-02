namespace AMJNReportSystem.Domain.Common.Contracts
{
    public abstract class AuditableEntity : AuditableEntity<Guid>
    {
    }

    public abstract class AuditableEntity<T> : BaseEntity<T>, IAuditableEntity, ISoftDelete
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get;  set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }

        protected AuditableEntity()
        {
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }
    }
}

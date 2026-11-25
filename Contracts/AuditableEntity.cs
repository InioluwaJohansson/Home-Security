namespace Home_Security.Contracts;

public abstract class AuditableEntity : BaseEntity, IAuditableEntity, ISoftDelete
{
    public int CreatedBy{get; set;} = 0;
    public DateTime CreatedOn{get; set;} = DateTime.UtcNow;
    public int LastModifiedBy{get; set;} = 0;
    public DateTime? LastModifiedOn{get; set;} = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; } = DateTime.UtcNow;
    public int DeletedBy{get; set;} = 0;
    public bool IsDeleted{get; set;} = false;
}
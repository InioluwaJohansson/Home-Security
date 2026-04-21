namespace Home_Security.Models.DTOs;
public class CreateBaseDefaultDto
{
    public int RoomId { get; set; }
    public int SectionId { get; set; }    
}
public class BaseDefaultDto
{
    public bool IsActive { get; set; }
    public bool PowerActive { get; set; }
    public int RoomId { get; set; }
    public string RoomName { get; set; }
    public int SectionId { get; set; }
    public string SectionName { get; set; }
    public int CreatedBy { get; set; }
    public string CreatedByName { get; set; }
    public DateTime CreatedOn { get; set; }
    public int LastModifiedBy { get; set; }
    public string LastModifiedByName { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public int DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public int personId { get; set; }
    public string PeronName { get; set; }
}
public class BaseDefaultDtoOther
{
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public int DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public int personId { get; set; }
}

namespace Home_Security.Models.DTOs;
public class CreateBaseDefaultDto
{
    public int RoomId { get; set; }
    public int SectionId { get; set; }
    public bool IsHidden { get; set; }
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
    public bool IsHidden { get; set; }
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
public class UserBaseDefaultDto
{
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
public class AppNamesDetailList{
    public List<GenericNames> ApplianceIdNames { get; set; }
    public List<GenericNames> CameraIdNames { get; set; }
    public List<GenericNames> LightIdNames { get; set; }
    public List<GenericNames> WindowIdNames { get; set; }
    public List<GenericNames> DoorIdNames { get; set; }
    public List<GenericNames> ExternalIdNames { get; set; }
    public List<GenericNames> PersonIdNames { get; set; }
    public List<GenericIdNames> ContactCategoryIdNames { get; set; }
    public List<GenericIdNames> ActionIdNames { get; set; }
    public List<GenericIdNames> ApplianceType { get; set; }
    public List<GenericIdNames> Gender { get; set; }
    public List<GenericIdNames> DoorType { get; set; }
    public List<GenericIdNames> FacilityType { get; set; }
    public List<GenericIdNames> Role { get; set; }
}
public class GenericNames : GenericIdNames{
    public bool IsHidden { get; set; }
    public string ImageUrl { get; set; }
}

using Home_Security.Models.Enums;

namespace Home_Security.Models.DTOs;
public class CreateActionDto
{
    public string ActionName { get; set; }
    public string Description { get; set; }
    public int personId { get; set; }
}
public class UpdateActionDto
{
    public int Id { get; set; }
    public int personId { get; set; }
    public string ActionName { get; set; }
    public string Description { get; set; }
}
public class CreateActionStepDto
{
    public int ActionId  { get; set; }
    public FacilityType FacilityType { get; set; }
    public int FacilityTypeId { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public bool IsActive { get; set; }
}
public class UpdateActionStepDto
{
    public int Id { get; set; }
    public int ActionId  { get; set; }
    public FacilityType FacilityType { get; set; }
    public int FacilityTypeId { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public bool IsActive { get; set; }
}
public class GetActionStepDto
{
    public int Id { get; set; }
    public int ActionId  { get; set; }
    public FacilityType FacilityType { get; set; }
    public int FacilityTypeId { get; set; }
    public int BrightnessLevel { get; set; }
    public bool IsLocked { get; set; }
    public bool IsOpen { get; set; }
    public bool IsActive { get; set; }
}
public class GetActionDto : UserBaseDefaultDto
{
    public int Id { get; set; }
    public string ActionId { get; set; }
    public string ActionName { get; set; }
    public string ActionDescription { get; set; }
    public bool ActionActive { get; set; }
    public List<GetActionStepDto> GetActionStepDtos { get; set; }
}
public class ActionResponseModel : BaseResponse
{
    public GetActionDto Data { get; set; }
}
public class ActionsResponseModel : BaseResponse
{
    public ICollection<GetActionDto> Data { get; set; } = new HashSet<GetActionDto>();
}
namespace Home_Security.Models.DTOs;
public class CreateActionTypeDto
{
    public string ActionTypeName { get; set; }
    public string ActionDescription { get; set; }
    public int personId { get; set; }
}
public class GetActionTypeDto
{
    public int Id { get; set; }
    public string ActionTypeId { get; set; }
    public string ActionTypeName { get; set; }
    public string ActionDescription { get; set; }
}
public class UpdateActionTypeDto
{
    public int Id { get; set; }
    public int personId { get; set; }
    public string ActionTypeName { get; set; }
    public string ActionDescription { get; set; }
}
public class ActionTypeResponseModel : BaseResponse
{
    public GetActionTypeDto Data { get; set; }
}
public class ActionTypesResponseModel : BaseResponse
{
    public ICollection<GetActionTypeDto> Data { get; set; } = new HashSet<GetActionTypeDto>();
}
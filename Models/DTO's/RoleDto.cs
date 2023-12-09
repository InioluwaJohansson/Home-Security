namespace Home_Security.Models.DTOs;
public class CreateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
public class GetRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
public class UpdateRoleDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
public class RoleResponseModel : BaseResponse
{
    public GetRoleDto Data { get; set; }
}
public class RolesResponseModel : BaseResponse
{
    public List<GetRoleDto> Data { get; set; } = new List<GetRoleDto>();
}

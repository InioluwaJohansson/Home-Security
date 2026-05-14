namespace Home_Security.Models.DTOs;
public class CreateExternalDto : CreateBaseDefaultDto
{
    public string ExternalName { get; set; }
    public List<int> ActionIds { get; set; }
}
public class UpdateExternalDto : CreateBaseDefaultDto
{
    public int Id { get; set; }
    public string ExternalName { get; set; }
    public List<int> ActionIds { get; set; }
}
public class GetExternalDto : BaseDefaultDto
{
    public int Id { get; set; }
    public string ExternalsName { get; set; }
    public string ExternalsId { get; set; }
    public bool IsTriggered { get; set; }
    public List<int> ActionIds { get; set; }
}
public class ExternalResponseModel : BaseResponse
{
    public GetExternalDto Data { get; set; }
}
public class ExternalsResponseModel : BaseResponse
{
    public ICollection<GetExternalDto> Data { get; set; } = new HashSet<GetExternalDto>();
}
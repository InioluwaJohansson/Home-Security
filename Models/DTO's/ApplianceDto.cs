using Home_Security.Models.Enums;

namespace Home_Security.Models.DTOs;
public class CreateApplianceDto : CreateBaseDefaultDto
{
    public string ApplianceName { get; set; }
    public ApplianceType ApplianceType { get; set; }
}
public class UpdateApplianceDto : CreateBaseDefaultDto
{
    public int Id { get; set; }
    public string ApplianceName { get; set; }
    public ApplianceType ApplianceType { get; set; }
    public bool IsActive { get; set; }
}
public class GetApplianceDto : BaseDefaultDto
{
    public int Id { get; set; }
    public string ApplianceName { get; set; }
    public string ApplianceId { get; set; }
    public string ApplianceType { get; set; }
}
public class ApplianceResponseModel : BaseResponse
{
    public GetApplianceDto Data { get; set; }
}
public class AppliancesResponseModel : BaseResponse
{
    public ICollection<GetApplianceDto> Data { get; set; } = new HashSet<GetApplianceDto>();
}
using Home_Security.Entities;
namespace Home_Security.Models.DTOs;
public class CreateLogDto
{
    public int PersonId { get; set; }
    public string ActionType { get; set; }
    public string LogDetails { get; set; }
}
public class GetLogDto
{
    public int Id { get; set; }
    public GetPersonDto GetPersonDto { get; set; }
    public int PersonId { get; set; }
    public string ActionType { get; set; }
    public DateTime TimeOfAction { get; set; }
    public string LogDetails { get; set; }
}
public class LogResponseModel : BaseResponse
{
    public GetLogDto Data { get; set; }
}
public class LogsResponseModel : BaseResponse
{
    public ICollection<GetLogDto> Data { get; set; } = new HashSet<GetLogDto>();
}
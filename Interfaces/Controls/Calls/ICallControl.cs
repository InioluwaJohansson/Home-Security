using Home_Security.Models.CallDTO;
using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls.Calls;
public interface ICallControl
{
    Task<CallDto> StartCall(GetAuthControlInfoDto getAuthControlInfoDto, StartCallDto dto);
    Task<BaseResponse> AcceptCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId);
    Task<BaseResponse> RejectCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId);
    Task<CallDto> GetCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId);
    Task<BaseResponse> EndCall(GetAuthControlInfoDto getAuthControlInfoDto, int callId);
}
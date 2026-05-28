using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface IFingerPrintControl
{
    public Task<BaseResponse> CreateFingerPrint(GetAuthControlInfoDto getAuthControlInfoDto, CreateFingerPrintDto createFingerPrintDto);
}
using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IFingerPrintService
{
    public Task<FingerPrintResponseModel> CreateFingerPrint(CreateFingerPrintDto createFingerPrintDto, int personId);
    public Task<FingerPrintResponseModel> VerifyFingerPrint(byte [] data);
    public Task ReceiveFingerPrint(FingerPrintDataDto fingerPrintDataDto);
    public Task<BaseResponse> RequestFingerPrint(int personId, int hardwareId);
}
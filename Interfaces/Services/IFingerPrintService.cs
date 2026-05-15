using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IFingerPrintService
{
    public Task<FingerPrintResponseModel> CreateFingerPrint(CreateFingerPrintDto createFingerPrintDto);
    public Task<FingerPrintResponseModel> VerifyFingerPrint(byte [] data);
}
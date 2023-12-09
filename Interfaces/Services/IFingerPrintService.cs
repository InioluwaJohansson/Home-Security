using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface IFingerPrintService
{
    public Task<FingerPrintResponseModel> VerifyFingerPrint(BinaryData data);
}
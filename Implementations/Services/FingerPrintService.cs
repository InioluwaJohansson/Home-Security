using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class FingerPrintService : IFingerPrintService
{
    IFingerPrintRepo _fingerPrintRepo;
    public FingerPrintService(IFingerPrintRepo fingerPrintRepo)
    {
        _fingerPrintRepo = fingerPrintRepo;
    }
    public async Task<FingerPrintResponseModel> VerifyFingerPrint(BinaryData data)
    {
        var fingerprint = await _fingerPrintRepo.Get(x => x.FingerPrintEncoding.Equals(data.ToString()) && x.IsDeleted == false);
        if (data != null)
        {
            return new FingerPrintResponseModel
            {
                Data = new GetFingerPrintDto()
                {
                    Id = fingerprint.Id,
                    FingerPrintId = fingerprint.FingerPrintId,
                    PersonId = fingerprint.PersonId,
                },
                Status = true,
                Message = "FingerPrint Verified"
            };
        }
        return new FingerPrintResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Find FingerPrint!"
        };
    }
}
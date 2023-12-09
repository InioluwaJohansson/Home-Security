using Home_Security.Context;
using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
namespace Home_Security.Interfaces.Repositories;
public interface IFingerPrintRepo : IRepo<FingerPrint>
{
    public Task<FingerPrint> GetFingerPrintById(int id);
    public Task<FingerPrint> GetFingerPrintByBinaryData(BinaryData data);
}
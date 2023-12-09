using Home_Security.Models.DTOs;
namespace Home_Security.Interfaces.Services;
public interface ISectionService
{
    public Task<BaseResponse> CreateSection(int personId, CreateSectionDto createSectionDto);
    public Task<BaseResponse> UpdateSection(int personId, UpdateSectionDto updateSectionDto);
    public Task<SectionResponseModel> GetSectionById(int id);
    public Task<SectionsResponseModel> GetSectionBySectionName(string SectionName);
    public Task<SectionsResponseModel> GetAllSections();
    public Task<BaseResponse> Delete(int id, int personId);
}
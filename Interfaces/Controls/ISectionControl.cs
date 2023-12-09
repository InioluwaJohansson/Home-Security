using Home_Security.Models.DTOs;

namespace Home_Security.Interfaces.Controls;
public interface ISectionControl
{
    public Task<BaseResponse> CreateSection(GetAuthControlInfoDto getAuthControlInfoDto, CreateSectionDto createSectionDto);
    public Task<BaseResponse> UpdateSection(GetAuthControlInfoDto getAuthControlInfoDto, UpdateSectionDto updateSectionDto);
    public Task<SectionResponseModel> GetSectionById(GetAuthControlInfoDto getAuthControlInfoDto, int id);
    public Task<SectionsResponseModel> GetSectionBySectionName(GetAuthControlInfoDto getAuthControlInfoDto, string sectionName);
    public Task<SectionsResponseModel> GetAllSections(GetAuthControlInfoDto getAuthControlInfoDto);
    public Task<BaseResponse> DeleteSection(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId);
}

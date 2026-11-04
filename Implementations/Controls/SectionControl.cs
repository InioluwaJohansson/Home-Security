using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Controls;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
namespace Home_Security.Implementations.Controls;
public class SectionControl : ISectionControl
{
    IAuthControl _authControl;
    ISectionService _sectionService;
    ILogService _logService;
    public SectionControl(IAuthControl authControl, ISectionService sectionService, ILogService logService)
    {
        _authControl = authControl;
        _sectionService = sectionService;
        _logService = logService;
    }
    public async Task<BaseResponse> CreateSection(GetAuthControlInfoDto getAuthControlInfoDto, CreateSectionDto createSectionDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var section = await _sectionService.CreateSection(auth.Id, createSectionDto);
            if (section.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Create";
                log.LogDetails = $"{createSectionDto.SectionName} Section was newly added. Awaiting Physical Interfacing!";
                log.FacilityType = FacilityTypeApp.Section;
                await _logService.CreateLog(log);
            }
            return section;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<BaseResponse> UpdateSection(GetAuthControlInfoDto getAuthControlInfoDto, UpdateSectionDto updateSectionDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var section = await _sectionService.UpdateSection(auth.Id, updateSectionDto);
            if (section.Status == true)
            {
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Update";
                log.LogDetails = $"{updateSectionDto.SectionName} Section was recently updated!";
                log.FacilityType = FacilityTypeApp.Section;
                log.FacilityId = updateSectionDto.Id;
                await _logService.CreateLog(log);
            }
            return section;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public async Task<SectionResponseModel> GetSectionById(GetAuthControlInfoDto getAuthControlInfoDto, int id)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var section = await _sectionService.GetSectionById(id);
            if (section.Data.IsHidden && auth.Role == Role.Owner || auth.Role == Role.Wife)
                return section;
            else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
            {
                return new SectionResponseModel()
                {
                    Data = null,
                    Status = _authControl.AuthFaliure().Status,
                    Message = "Unauthorized Action"
                };
            }
        }
        return new SectionResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<SectionsResponseModel> GetSectionBySectionName(GetAuthControlInfoDto getAuthControlInfoDto, string sectionName)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife)
        {
            var section = await _sectionService.GetSectionBySectionName(sectionName);
            section.Data = section.Data.Select(x => DtoRefactor(x)).ToList();
            return section;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var section = await _sectionService.GetSectionBySectionName(sectionName);
            section.Data = section.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return section;
        }
        return new SectionsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<SectionsResponseModel> GetAllSections(GetAuthControlInfoDto getAuthControlInfoDto)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner || auth.Role == Role.Wife || auth.Role == Role.Child)
        {
            var section = await _sectionService.GetAllSections();
            section.Data = section.Data.Select(x => DtoRefactor(x)).ToList();
            return section;
        }
        else if (auth.Status != false && auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var section = await _sectionService.GetAllSections();
            section.Data = section.Data.Select(x => DtoRefactor(x)).Where(x => x.IsHidden == false).ToList();
            return section;
        }
        return new SectionsResponseModel()
        {
            Data = null,
            Status = _authControl.AuthFaliure().Status,
            Message = _authControl.AuthFaliure().Message
        };
    }
    public async Task<BaseResponse> DeleteSection(GetAuthControlInfoDto getAuthControlInfoDto, int sectionId)
    {
        var auth = await _authControl.GetAuthDetails(getAuthControlInfoDto.PersonId, getAuthControlInfoDto.AuthorizationCode);
        if (auth.Status != false && auth.Role == Role.Owner)
        {
            var section = await _sectionService.Delete(sectionId, auth.Id);
            if (section.Status == true)
            {
                var getSection = _authControl.GetSectionName(sectionId);
                var log = _authControl.CreateLog(auth.Id);
                log.ActionType = "Delete";
                log.LogDetails = $"{getSection} Section was Deleted!";
                log.FacilityType = FacilityTypeApp.Section;
                log.FacilityId = sectionId;
                await _logService.CreateLog(log);
            }
            return section;
        }
        else if (auth.Status != false && auth.Role == Role.Child || auth.Role == Role.Wife || auth.Role == Role.Relative || auth.Role == Role.Visitor)
        {
            var fail = _authControl.AuthFaliure();
            fail.Message = "Unauthorized Action";
            return fail;
        }
        return _authControl.AuthFaliure();
    }
    public GetSectionDto DtoRefactor(GetSectionDto x)
    {
        x.LastModifiedByName = _authControl.GetUserPersonName(x.LastModifiedBy);
        x.PeronName = _authControl.GetUserPersonName(x.personId ?? 0);
        x.CreatedByName = _authControl.GetUserPersonName(x.CreatedBy);
        return x;
    }
}
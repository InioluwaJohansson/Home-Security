using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
namespace Home_Security.Implementations.Services;
public class WindowService : IWindowService
{
    IWindowRepo _windowRepo;
    public WindowService(IWindowRepo windowRepo)
    {
        _windowRepo = windowRepo;
    }
    public async Task<BaseResponse> CreateWindow(CreateWindowDto createWindowDto)
    {
        if (createWindowDto != null)
        {
            var window = new Window()
            {
                WindowId = $"APPLIANCE{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                WindowName = createWindowDto.WindowName,
                IsLocked = createWindowDto.IsLocked,
                IsOpen = createWindowDto.IsOpen,
                OpenedBy = createWindowDto.OpenedBy,
                LockedBy = createWindowDto.LockedBy,
                UnlockedBy = createWindowDto.UnlockedBy,
                IsActive = createWindowDto.IsActive,
                PowerActive = createWindowDto.PowerActive,
                SectionId = createWindowDto.SectionId,
                RoomId = createWindowDto.RoomId,
                IsDeleted = false,
                CreatedBy = createWindowDto.personId,
                LastModifiedBy = createWindowDto.personId,
                LastModifiedOn = DateTime.Now
            };
            await _windowRepo.Create(window);
            return new BaseResponse()
            {
                Status = true,
                Message = "Window Successfully Added!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Window!"
        };
    }
    public async Task<BaseResponse> UpdateWindow(UpdateWindowDto updateWindowDto)
    {
        if (updateWindowDto != null)
        {
            var window = await _windowRepo.Get(x => x.Id == updateWindowDto.Id && x.IsDeleted == false);
            window.WindowName = updateWindowDto.WindowName ?? window.WindowName;
            window.IsActive = updateWindowDto.IsActive;
            window.PowerActive = updateWindowDto.PowerActive;
            window.LastModifiedBy = updateWindowDto.personId;
            window.IsLocked = updateWindowDto.IsLocked;
            window.IsOpen = updateWindowDto.IsOpen;
            window.OpenedBy = updateWindowDto.OpenedBy;
            window.LockedBy = updateWindowDto.LockedBy;
            window.UnlockedBy = updateWindowDto.UnlockedBy;
            window.LastModifiedOn = DateTime.Now;
            await _windowRepo.Update(window);
            return new BaseResponse()
            {
                Status = true,
                Message = "Window Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Window!"
        };
    }
    public async Task<WindowResponseModel> GetById(int windowId)
    {
        var window = await _windowRepo.Get(x => x.Id == windowId && x.IsDeleted == false);
        if (window != null)
        {
            return new WindowResponseModel()
            {
                Data = GetDetails(window),
                Status = true,
                Message = "Window Retrieved Successfully!"
            };
        }
        return new WindowResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Window!"
        };
    }
    public async Task<WindowsResponseModel> GetAllWindows()
    {
        var windows = await _windowRepo.GetByExpression(x => x.IsDeleted == false);
        if (windows != null)
        {
            return new WindowsResponseModel()
            {
                Data = windows.Select(x => GetDetails(x)).ToList(),
                Message = "Windows Retrieved Successfully!",
                Status = true
            };
        }
        return new WindowsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Windows!",
            Status = false
        };
    }
    public async Task<WindowsResponseModel> GetAllWindowsByRoomId(int roomId)
    {
        var windows = await _windowRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);
        if (windows != null)
        {
            return new WindowsResponseModel()
            {
                Data = windows.Select(x => GetDetails(x)).ToList(),
                Message = "Windows Retrieved Successfully!",
                Status = true
            };
        }
        return new WindowsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Windows!",
            Status = false
        };
    }
    public async Task<WindowsResponseModel> GetAllWindowsBySectionId(int sectionId)
    {
        var windows = await _windowRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);
        if (windows != null)
        {
            return new WindowsResponseModel()
            {
                Data = windows.Select(x => GetDetails(x)).ToList(),
                Message = "Windows Retrieved Successfully!",
                Status = true
            };
        }
        return new WindowsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Windows!",
            Status = false
        };
    }
    public async Task<BaseResponse> Delete(int windowId, int personId)
    {
        var window = await _windowRepo.Get(x => x.Id == windowId);
        if (window != null)
        {
            window.IsActive = false;
            window.PowerActive = false;
            window.LastModifiedBy = personId;
            window.LastModifiedOn = DateTime.Now;
            window.DeletedOn = DateTime.Now;
            window.DeletedBy = personId;
            window.IsDeleted = true;
            await _windowRepo.Update(window);
            return new BaseResponse()
            {
                Status = true,
                Message = "Window Deleted Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Window!"
        };
    }
    public GetWindowDto GetDetails(Window window)
    {
        return new GetWindowDto()
        {
            Id = window.Id,
            WindowName = window.WindowName,
            IsLocked = window.IsLocked,
            IsOpen = window.IsOpen,
            OpenedBy = window.OpenedBy,
            LockedBy = window.LockedBy,
            UnlockedBy = window.UnlockedBy,
            IsActive = window.IsActive,
            PowerActive = window.PowerActive,
            RoomId = window.RoomId,
            SectionId = window.SectionId,
            WindowId = window.WindowId,
            LastModifiedBy = window.LastModifiedBy,
            DeletedBy = window.DeletedBy,
            CreatedBy = window.CreatedBy,
            LastModifiedOn = window.LastModifiedOn,
            CreatedOn = window.CreatedOn,
            IsDeleted = window.IsDeleted,
        };
    }
}
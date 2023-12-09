using Home_Security.Entities;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;

namespace Home_Security.Implementations.Services;
public class DoorService : IDoorService
{
    IDoorRepo _doorRepo;
    public DoorService(IDoorRepo doorRepo)
    {
        _doorRepo = doorRepo;
    }
    public async Task<BaseResponse> CreateDoor(CreateDoorDto createDoorDto)
    {
        if (createDoorDto != null)
        {
            var door = new Door()
            {
                DoorId = $"DOOR{Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5).ToUpper()}",
                DoorName = createDoorDto.DoorName,
                DoorType = createDoorDto.DoorType,
                IsLocked = createDoorDto.IsLocked,
                IsOpen = createDoorDto.IsOpen,
                OpenedBy = createDoorDto.OpenedBy,
                LockedBy = createDoorDto.LockedBy,
                UnlockedBy = createDoorDto.UnlockedBy,
                IsActive = createDoorDto.IsActive,
                PowerActive = createDoorDto.PowerActive,
                SectionId = createDoorDto.SectionId,
                RoomId = createDoorDto.RoomId,
                IsDeleted = false,
                CreatedBy = createDoorDto.personId,
                LastModifiedBy = createDoorDto.personId,
                LastModifiedOn = DateTime.Now
            };
            await _doorRepo.Create(door);
            return new BaseResponse()
            {
                Status = true,
                Message = "Door Successfully Added!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Add Door!"
        };
    }
    public async Task<BaseResponse> UpdateDoor(UpdateDoorDto updateDoorDto)
    {
        if (updateDoorDto != null)
        {
            var door = await _doorRepo.Get(x => x.Id == updateDoorDto.Id);
            door.DoorName = updateDoorDto.DoorName ?? door.DoorName;
            door.IsActive = updateDoorDto.IsActive;
            door.PowerActive = updateDoorDto.PowerActive;
            door.LastModifiedBy = updateDoorDto.personId;
            door.IsLocked = updateDoorDto.IsLocked;
            door.IsOpen = updateDoorDto.IsOpen;
            door.OpenedBy = updateDoorDto.OpenedBy;
            door.LockedBy = updateDoorDto.LockedBy;
            door.UnlockedBy = updateDoorDto.UnlockedBy;
            door.LastModifiedOn = DateTime.Now;
            await _doorRepo.Update(door);
            return new BaseResponse()
            {
                Status = true,
                Message = "Door Updated Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Update Door!"
        };
    }
    public async Task<DoorResponseModel> GetById(int doorId)
    {
        var door = await _doorRepo.Get(x => x.Id == doorId && x.IsDeleted == false);
        if (door != null)
        {
            return new DoorResponseModel()
            {
                Data = GetDetails(door),
                Status = true,
                Message = "Door Retrieved Successfully!"
            };
        }
        return new DoorResponseModel()
        {
            Data = null,
            Status = false,
            Message = "Unable To Retrieve Door!"
        };
    }
    public async Task<DoorsResponseModel> GetAllDoors()
    {
        var doors = await _doorRepo.GetByExpression(x => x.IsDeleted == false);
        if (doors != null)
        {
            return new DoorsResponseModel()
            {
                Data = doors.Select(x => GetDetails(x)).ToList(),
                Message = "Doors Retrieved Successfully!",
                Status = true
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Doors!",
            Status = false
        };
    }
    public async Task<DoorsResponseModel> GetAllDoorsByDoorType(DoorType doorType)
    {
        var doors = await _doorRepo.GetByExpression(x => x.DoorType == doorType && x.IsDeleted == false);
        if (doors != null)
        {
            return new DoorsResponseModel()
            {
                Data = doors.Select(x => GetDetails(x)).ToList(),
                Message = "Doors Retrieved Successfully!",
                Status = true
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Doors!",
            Status = false
        };
    }
    public async Task<DoorsResponseModel> GetAllDoorsByRoomId(int roomId)
    {
        var doors = await _doorRepo.GetByExpression(x => x.RoomId == roomId && x.IsDeleted == false);
        if (doors != null)
        {
            return new DoorsResponseModel()
            {
                Data = doors.Select(x => GetDetails(x)).ToList(),
                Message = "Doors Retrieved Successfully!",
                Status = true
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Doors!",
            Status = false
        };
    }
    public async Task<DoorsResponseModel> GetAllDoorsBySectionId(int sectionId)
    {
        var doors = await _doorRepo.GetByExpression(x => x.SectionId == sectionId && x.IsDeleted == false);
        if (doors != null)
        {
            return new DoorsResponseModel()
            {
                Data = doors.Select(x => GetDetails(x)).ToList(),
                Message = "Doors Retrieved Successfully!",
                Status = true
            };
        }
        return new DoorsResponseModel()
        {
            Data = null,
            Message = "Unable To Retrieve Doors!",
            Status = false
        };
    }
    public async Task<BaseResponse> Delete(int doorId, int personId)
    {
        var door = await _doorRepo.Get(x => x.Id == doorId);
        if (door != null)
        {
            door.IsActive = false;
            door.PowerActive = false;
            door.LastModifiedBy = personId;
            door.LastModifiedOn = DateTime.Now;
            door.DeletedOn = DateTime.Now;
            door.DeletedBy = personId;
            door.IsDeleted = true;
            await _doorRepo.Update(door);
            return new BaseResponse()
            {
                Status = true,
                Message = "Door Deleted Successfully!"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable To Delete Door!"
        };
    }
    public GetDoorDto GetDetails(Door door)
    {
        return new GetDoorDto()
        {
            Id = door.Id,
            DoorName = door.DoorName,
            DoorType = door.DoorType,
            IsLocked = door.IsLocked,
            IsOpen = door.IsOpen,
            OpenedBy = door.OpenedBy,
            LockedBy = door.LockedBy,
            UnlockedBy = door.UnlockedBy,
            IsActive = door.IsActive,
            PowerActive = door.PowerActive,
            RoomId = door.RoomId,
            SectionId = door.SectionId,
            DoorId = door.DoorId,
            LastModifiedBy = door.LastModifiedBy,
            DeletedBy = door.DeletedBy,
            CreatedBy = door.CreatedBy,
            LastModifiedOn = door.LastModifiedOn,
            CreatedOn = door.CreatedOn,
            IsDeleted = door.IsDeleted,
        };
    }
}
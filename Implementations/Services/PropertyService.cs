using System.Text.Json;
using Home_Security.Entities;
using Home_Security.Entities.Identity;
using Home_Security.Interfaces.Repositories;
using Home_Security.Interfaces.Services;
using Home_Security.Models.DTOs;
using Home_Security.Models.Enums;
using Home_Security.RealTimeServices;

namespace Home_Security.Implementations.Services;
public class PropertyService : IPropertyService
{
    IPropertyRepo _propertyRepo;
    IRealtimeNotificationService _realtime;
    public PropertyService(IPropertyRepo propertyRepo, IRealtimeNotificationService realtime)
    {
        _propertyRepo = propertyRepo;
        _realtime = realtime;
    }
    public async Task<BaseResponse> CreateProperty(CreatePropertyDto createPropertyDto, int personId)
    {
        var getProperty = await _propertyRepo.Get(x => x.PropertyName == createPropertyDto.PropertyName && x.IsDeleted == false);
        var getPropertyCount = await _propertyRepo.GetByExpression(x => x.IsDeleted == false);
        if(getProperty == null && getPropertyCount.Count < 1)
        {
            var property = new Property()
            {
                PropertyName = createPropertyDto.PropertyName,
                JsonData = JsonSerializer.Serialize(createPropertyDto.JsonData),
            };
            property = await _propertyRepo.Create(property);
            await _realtime.NotifyAll("PropertyCreated", GetDetails(property));
            return new BaseResponse()
            {
                Status = true,
                Message = "Property Created"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable to Create Property"
        };
    }
    public async Task<BaseResponse> UpdateProperty(UpdatePropertyDto updatePropertyDto, int personId)
    {
        var property = await _propertyRepo.Get(x => x.Id == updatePropertyDto.Id && x.IsDeleted == false);
        if(property != null)
        {
            property.PropertyName = updatePropertyDto.PropertyName ?? property.PropertyName;
            property.JsonData = JsonSerializer.Serialize(updatePropertyDto.JsonData) ?? property.JsonData;
            property.LastModifiedBy = personId;
            property.LastModifiedOn = DateTime.UtcNow;
            await _propertyRepo.Update(property);
            await _realtime.NotifyAll("PropertyUpdated", GetDetails(property));
            return new BaseResponse()
            {
                Status = true,
                Message = "Property Updated"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable to Update Property"
        };
    }
    public async Task<GetPropertyDto> GetById(int id)
    {
        var property = await _propertyRepo.Get(x => x.IsDeleted == false);
        if(property != null)
        {
            return GetDetails(property);
        }
        return new GetPropertyDto();
    }
    public async Task<List<GetPropertyDto>> GetAllProperties()
    {
        var properties = await _propertyRepo.GetByExpression(x => x.IsDeleted == false);
        if(properties != null)
        {
            return properties.Select(GetDetails).ToList();
        }
        return new List<GetPropertyDto>();
    }
    public async Task<BaseResponse> DeleteProperty(int propertyId, int personId)
    {
        var property = await _propertyRepo.Get(x => x.Id == propertyId && x.IsDeleted == false);
        if(property != null)
        {
            property.LastModifiedBy = personId;
            property.DeletedBy = personId;
            property.DeletedOn = DateTime.UtcNow;
            property.LastModifiedOn = DateTime.UtcNow;
            property.IsDeleted = false;
            await _propertyRepo.Update(property);
            await _realtime.NotifyAll("PropertyDeleted", new
            {
                Id = property.Id
            });
            return new BaseResponse()
            {
                Status = true,
                Message = "Property Deleted"
            };
        }
        return new BaseResponse()
        {
            Status = false,
            Message = "Unable to Update Property"
        };
    }
    private GetPropertyDto GetDetails(Property property)
    {
        return new GetPropertyDto()
        {
            Id = property.Id,
            PropertyName = property.PropertyName,
            JsonData = JsonDocument.Parse(property.JsonData)
        };
    }
}